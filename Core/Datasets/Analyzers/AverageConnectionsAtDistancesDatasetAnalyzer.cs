using socializer.Data.Domain.Entities;

namespace socializer.Core.Datasets.Analyzers;

public class AverageConnectionsAtDistancesDatasetAnalyzer : IAverageConnectionsAtDistancesDatasetAnalyzer
{
    public Dictionary<int, double> Calculate(ICollection<SocialConnection> connections)
    {
        var graph = new Dictionary<int, HashSet<int>>();
        foreach (var connection in connections)
        {
            if (!graph.TryGetValue(connection.Peer1Id, out HashSet<int>? value))
            {
                value = ([]);
                graph[connection.Peer1Id] = value;
            }

            if (!graph.TryGetValue(connection.Peer2Id, out HashSet<int>? value2))
            {
                value2 = ([]);
                graph[connection.Peer2Id] = value2;
            }

            value.Add(connection.Peer2Id);
            value2.Add(connection.Peer1Id);
        }

        var distanceSums = new Dictionary<int, int>();
        var distanceCounts = new Dictionary<int, int>();

        foreach (var user in graph.Keys)
        {
            var distances = BFS(graph, user);

            foreach (var (distance, count) in distances)
            {
                if (!distanceSums.ContainsKey(distance))
                {
                    distanceSums[distance] = 0;
                    distanceCounts[distance] = 0;
                }
                distanceSums[distance] += count;
                distanceCounts[distance]++;
            }
        }

        var averages = new Dictionary<int, double>();
        foreach (var distance in distanceSums.Keys)
        {
            averages[distance] = (double)distanceSums[distance] / distanceCounts[distance];
        }

        return averages;
    }

    private Dictionary<int, int> BFS(Dictionary<int, HashSet<int>> graph, int start)
    {
        var queue = new Queue<(int node, int distance)>();
        var visited = new HashSet<int> { start };
        var distanceCounts = new Dictionary<int, int>();

        queue.Enqueue((start, 0));

        while (queue.Count > 0)
        {
            var (node, distance) = queue.Dequeue();

            if (!distanceCounts.ContainsKey(distance))
                distanceCounts[distance] = 0;
            distanceCounts[distance]++;

            foreach (var neighbor in graph[node])
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue((neighbor, distance + 1));
                }
            }
        }

        return distanceCounts;
    }
}
