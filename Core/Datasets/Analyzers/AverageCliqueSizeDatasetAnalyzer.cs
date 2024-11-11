using socializer.Data.Domain.Entities;

namespace socializer.Core.Datasets.Analyzers;

public class AverageCliqueSizeDatasetAnalyzer : IAverageCliqueSizeDatasetAnalyzer
{
    public double Calculate(ICollection<SocialConnection> connections)
    {
        var graph = new Dictionary<int, List<int>>();
        var cliques = new List<List<int>>();

        foreach (var connection in connections)
        {
            if (!graph.ContainsKey(connection.Peer1Id))
            {
                graph[connection.Peer1Id] = [];
            }

            if (!graph.ContainsKey(connection.Peer2Id))
            {
                graph[connection.Peer2Id] = [];
            }

            graph[connection.Peer1Id].Add(connection.Peer2Id);
            graph[connection.Peer2Id].Add(connection.Peer1Id);
        }

        BronKerbosch(graph, [], [.. graph.Keys], [], cliques);

        return cliques.Count > 0 ? cliques.Average(clique => clique.Count) : 0;
    }

    private static void BronKerbosch(Dictionary<int, List<int>> graph, List<int> R, List<int> P, List<int> X, List<List<int>> cliques)
    {
        if (!P.Any() && !X.Any())
        {
            cliques.Add(new List<int>(R));
            return;
        }

        var PSet = new List<int>(P);
        foreach (var v in PSet)
        {
            var newR = new List<int>(R) { v };
            var newP = P.Intersect(graph[v]).ToList();
            var newX = X.Intersect(graph[v]).ToList();
            BronKerbosch(graph, newR, newP, newX, cliques);

            P.Remove(v);
            X.Add(v);
        }
    }
}