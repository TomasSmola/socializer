using socializer.Data.Domain.Entities;

namespace socializer.Core.Datasets.Analyzers
{
    public interface IAverageCliqueSizeDatasetAnalyzer
    {
        double Calculate(ICollection<SocialConnection> connections);
    }
}