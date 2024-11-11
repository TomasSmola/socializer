using socializer.Data.Domain.Entities;

namespace socializer.Core.Datasets.Analyzers;

public interface IAverageNumberOfConnectionsPerPeerAnalyzer
{
    double Calculate(ICollection<SocialConnection> connections);
}