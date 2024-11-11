namespace socializer.Data.Domain.Entities;

public class Dataset
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTimeOffset ImportTimestamp { get; set; }
}
