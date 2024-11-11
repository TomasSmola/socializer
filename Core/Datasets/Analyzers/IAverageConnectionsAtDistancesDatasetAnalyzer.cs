using socializer.Data.Domain.Entities;

namespace socializer.Core.Datasets.Analyzers
{
    public interface IAverageConnectionsAtDistancesDatasetAnalyzer
    {
        Dictionary<int, double> Calculate(ICollection<SocialConnection> socialConnections);
    }
}