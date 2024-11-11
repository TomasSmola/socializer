using socializer.Data.Domain.Entities;

namespace socializer.Core.Datasets.Analyzers;

public class NumberOfPeersAnalyzer : INumberOfPeersAnalyzer
{
    public int Calculate(ICollection<SocialConnection> socialConnections)
    {
        var peers = new HashSet<int>();
        foreach (var socialConnection in socialConnections)
        {
            peers.Add(socialConnection.Peer1Id);
            peers.Add(socialConnection.Peer2Id);
        }

        return peers.Count;
    }
}
