using ShipDataLoader.Contracts;

namespace ShipDataLoader
{
    public class FileProcessor(ILineParser lineParser, IDataLoader dataLoader) : IFileProcessor
    {
        private readonly ILineParser _lineParser = lineParser;
        private readonly IDataLoader _dataLoader = dataLoader;

        public static void MarkFileCompleted(string filePath)
        {
            string? directory = Path.GetDirectoryName(filePath);
            if (directory != null) {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string newFileName = $"{fileNameWithoutExtension}_completed_{timestamp}{extension}";
                string newFilePath = Path.Combine(directory, newFileName);

                File.Move(filePath, newFilePath);
            }
        }

        public async Task ProcessFileAsync(string filePath)
        {
            try
            {
                using (FileStream fs = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                using (StreamReader sr = new(fs))
                {
                    string? line;
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        var parsedData = _lineParser.ParseLine(line);
                        if (parsedData != null)
                        {
                            if (parsedData.SupplierId != null && parsedData.CartonBoxId != null)
                            {
                                await _dataLoader.AddBox(parsedData.SupplierId, parsedData.CartonBoxId);
                            }
                            else if (parsedData.PoNumber != null && parsedData.Isbn != null)
                            {
                                _dataLoader.AddBoxSpec(parsedData.PoNumber, parsedData.Isbn, parsedData.Quantity);
                            }
                        }                        
                    }
                    await _dataLoader.FlushDataAsync();
                }
                MarkFileCompleted(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");
            }
        }
    }
}
