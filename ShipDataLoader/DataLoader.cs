using ShipDataLoader.Contracts;
using System.Data;
using System.Data.SqlClient;

namespace ShipDataLoader
{
    public class BoxDataSpec
    {
        public string? PoNumber { get; set; }
        public string? Isbn { get; set; }
        public decimal Quantity { get; set; }
    }

    public class BoxData
    {
        public string? SupplierId { get; set; }
        public string? BoxId { get; set; }

        public required List<BoxDataSpec> BoxSpecs { get; set; }
    }
    public class DatabaseLoader : IDataLoader
    {
        private readonly List<BoxData> _boxesData = [];
        private readonly string _conStr = @"Data Source = (localdb)\ProjectModels; Initial Catalog = ShipDataDb; Integrated Security = True";
        private readonly string _boxesTableName = "Boxes";
        private readonly string _boxesSpecTableName = "BoxesSpec";
        private readonly int _boxFlushCount = 5;

        public async Task AddBox(string supplierId, string cartonBoxId)
        {
            if (_boxesData.Count > _boxFlushCount)
                await FlushDataAsync();
            _boxesData.Add(new BoxData {
                SupplierId = supplierId,
                BoxId = cartonBoxId,
                BoxSpecs = []
            });
            Console.WriteLine("Added box data");
        }

        public void AddBoxSpec(string poNumber, string isbn, decimal quantity)
        {
            _boxesData.Last()?.BoxSpecs.Add(new BoxDataSpec
                {
                    PoNumber = poNumber,
                    Isbn = isbn,
                    Quantity = quantity
                });
        }

        private static DataTable CreateBoxesTable()
        {
            var table = new DataTable();
            table.Columns.Add("BoxId", typeof(string));
            table.Columns.Add("SupplierId", typeof(string));
            return table;
        }

        private static DataTable CreateBoxesSpecTable()
        {
            var table = new DataTable();
            table.Columns.Add("BoxId", typeof(string));
            table.Columns.Add("PoNumber", typeof(string));
            table.Columns.Add("Isbn", typeof(string));
            table.Columns.Add("Quantity", typeof(decimal));
            return table;
        }

        private void PopulateTables(DataTable boxesTable, DataTable boxesSpecTable)
        {
            foreach (var boxData in _boxesData)
            {
                var dataRow = boxesTable.NewRow();
                dataRow["BoxId"] = boxData.BoxId;
                dataRow["SupplierId"] = boxData.SupplierId;
                boxesTable.Rows.Add(dataRow);

                foreach (var boxSpecData in boxData.BoxSpecs)
                {
                    var dataSpecRow = boxesSpecTable.NewRow();
                    dataSpecRow["BoxId"] = boxData.BoxId;
                    dataSpecRow["PoNumber"] = boxSpecData.PoNumber;
                    dataSpecRow["Isbn"] = boxSpecData.Isbn;
                    dataSpecRow["Quantity"] = boxSpecData.Quantity;
                    boxesSpecTable.Rows.Add(dataSpecRow);
                }
            }
        }

        private static async Task WriteToServerAsync(SqlBulkCopy bulkCopy, DataTable table, string tableName)
        {
            bulkCopy.DestinationTableName = tableName;
            await bulkCopy.WriteToServerAsync(table);
        }

        public void ClearBoxesData()
        {
            foreach (var boxData in _boxesData)
            {
                boxData.BoxSpecs.Clear();
            }
            _boxesData.Clear();
        }

        public async Task FlushDataAsync()
        {
            var boxesTable = CreateBoxesTable();
            var boxesSpecTable = CreateBoxesSpecTable();

            if ( _boxesData.Count > 0 )
            {
                try
                {
                    using SqlConnection connection = new(_conStr);
                    connection.Open();
                    using SqlBulkCopy bulkCopy = new(connection);
                    PopulateTables(boxesTable, boxesSpecTable);

                    await WriteToServerAsync(bulkCopy, boxesTable, _boxesTableName);
                    await WriteToServerAsync(bulkCopy, boxesSpecTable, _boxesSpecTableName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading data to Database: {ex.Message}");
                }
            }
            ClearBoxesData();
            Console.WriteLine("Flushed data");
        }
    }
}
