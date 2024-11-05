using ShipDataLoader.Contracts;

namespace ShipDataLoader
{

    public class ParsedData
    {
        public string? SupplierId { get; set; }
        public string? CartonBoxId { get; set; }
        public string? PoNumber { get; set; }
        public string? Isbn { get; set; }
        public decimal Quantity { get; set; }
    }

    public class LineParser : ILineParser
    {
        private static ParsedData ParseBoxLine(string line)
        {
            return new ParsedData
            {
                SupplierId = ExtractSubstring(line, 4, 11),
                CartonBoxId = ExtractSubstring(line, 96, 12)
            };
        }

        private static ParsedData ParseBoxSpecLine(string line)
        {
            return new ParsedData
            {
                PoNumber = ExtractSubstring(line, 5, 13),
                Isbn = ExtractSubstring(line, 41, 17),
                Quantity = Decimal.Parse(ExtractSubstring(line, 68, 15))
            };
        }

        private static string ExtractSubstring(string line, int startIndex, int length)
        {
            return line.Substring(startIndex, length).Trim();
        }

        public ParsedData? ParseLine(string line)
        {
            if (line.StartsWith("HDR"))
            {
                return ParseBoxLine(line);
            }
            else if (line.StartsWith("LINE"))
            {
                return ParseBoxSpecLine(line);
            }
            return null;
        }
    }
}
