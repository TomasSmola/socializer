using socializer.Data.Domain.Entities;

namespace socializer.Core.Datasets.Analyzers
{
    public interface INumberOfPeersAnalyzer
    {
        int Calculate(ICollection<SocialConnection> socialConnections);
    }
}