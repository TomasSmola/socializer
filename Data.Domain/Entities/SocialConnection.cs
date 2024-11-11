namespace socializer.Data.Domain.Entities;

public class SocialConnection
{
    public int Id { get; set; }
    public int DatasetId { get; set; }
    public int Peer1Id { get; set; }
    public int Peer2Id { get; set; }
}
