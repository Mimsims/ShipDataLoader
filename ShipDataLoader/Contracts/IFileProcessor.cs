namespace ShipDataLoader.Contracts 
{ 
    public interface IFileProcessor
    {
        Task ProcessFileAsync(string filePath);
    }
}
