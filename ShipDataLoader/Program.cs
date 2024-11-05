using ShipDataLoader.Contracts;

namespace ShipDataLoader
{
    class Program
    {
        static void Main()
        {
            string folderPath = @".\upload";
            IFileProcessor fileProcessor = new FileProcessor(new LineParser(), new DatabaseLoader());
            FileWatcher watcher = new(folderPath, fileProcessor);

            watcher.Start();

            Console.WriteLine("Monitoring folder for new files. Press [Enter] to exit.");
            Console.ReadLine();
        }
    }
}























//class Program
//{
//    static void Main()
//    {
//        string folderPath = @".\upload";
//        FileSystemWatcher watcher = new FileSystemWatcher(folderPath);

//        watcher.Created += OnNewFile;
//        watcher.EnableRaisingEvents = true;

//        Console.WriteLine("Monitoring folder for new files. Press [Enter] to exit.");
//        Console.ReadLine();
//    }

//    private static async void OnNewFile(object sender, FileSystemEventArgs e)
//    {
//        Console.WriteLine($"New file detected: {e.FullPath}");
//        await ProcessFileAsync(e.FullPath);
//    }

//    private static async Task ProcessFileAsync(string filePath)
//    {
//        try
//        {
//            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
//            using (StreamReader sr = new StreamReader(fs))
//            {
//                string line;
//                while ((line = await sr.ReadLineAsync()) != null)
//                {
//                    // Process each line and load into database
//                    await ParseAndLoadLineAsync(line);
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error processing file: {ex.Message}");
//        }
//    }

//    private static async Task ParseAndLoadLineAsync(string line)
//    {
//        // Parse the line based on your file structure
//        if (line.StartsWith("HDR"))
//        {
//            // Handle header line
//            string supplierId = line.Substring(4, 7).Trim();
//            string cartonBoxId = line.Substring(11, 10).Trim();
//            Console.WriteLine($"Header: Supplier ID = {supplierId}, Carton Box ID = {cartonBoxId}");
//        }
//        else if (line.StartsWith("LINE"))
//        {
//            // Handle product line
//            string poNumber = line.Substring(5, 10).Trim();
//            string isbn = line.Substring(15, 13).Trim();
//            int quantity = int.Parse(line.Substring(28, 5).Trim());
//            Console.WriteLine($"Line: PO Number = {poNumber}, ISBN = {isbn}, Quantity = {quantity}");

//            // Load into database (example using async method)
//            await LoadIntoDatabaseAsync(poNumber, isbn, quantity);
//        }
//    }

//    private static async Task LoadIntoDatabaseAsync(string poNumber, string isbn, int quantity)
//    {
//        // Example database loading logic
//        // Replace with actual database code
//        await Task.Run(() =>
//        {
//            Console.WriteLine($"Loading into database: PO Number = {poNumber}, ISBN = {isbn}, Quantity = {quantity}");
//            // Database logic here
//        });
//    }
//}
