namespace ShipDataLoader.Contracts 
{
    public interface ILineParser
    {
        ParsedData? ParseLine(string line);
    }
}
