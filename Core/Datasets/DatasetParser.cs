using System.Text;

namespace socializer.Core.Datasets;

public static class DatasetParser
{
    public static async IAsyncEnumerable<(int, int)> Parse(Stream input)
    {
        using var reader = new StreamReader(input, Encoding.UTF8);
        string? line;
        while ((line = await reader.ReadLineAsync()) is not null)
        {
            yield return ParseLine(line);
        }
    }
    private static (int, int) ParseLine(string line)
    {
        var parts = line.Split(' ');
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }
}
