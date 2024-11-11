using socializer.Data.Domain.Entities;

namespace socializer.Core.Datasets.Analyzers;

public class AverageNumberOfConnectionsPerPeerAnalyzer : IAverageNumberOfConnectionsPerPeerAnalyzer
{
    public double Calculate(ICollection<SocialConnection> connections)
    {
        var peers = new Dictionary<int, int>();
        foreach (var connection in connections)
        {
            addPeerConnection(peers, connection.Peer1Id);
            addPeerConnection(peers, connection.Peer2Id);
        }

        return peers.Values.Average();

        static void addPeerConnection(Dictionary<int, int> dict, int peerId)
        {
            if (!dict.ContainsKey(peerId))
            {
                dict[peerId] = 0;
            }

            dict[peerId]++;
        }
    }
}
