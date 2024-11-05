namespace ShipDataLoader.Contracts
{
    public interface IDataLoader
    {
        Task FlushDataAsync();
        Task AddBox(string supplierId, string cartonBoxId);
        void AddBoxSpec(string poNumber, string isbn, decimal quantity);
    }
}