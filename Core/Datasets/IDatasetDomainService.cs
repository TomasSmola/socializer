
using socializer.Application.Dtos;

namespace socializer.Core.Datasets;

public interface IDatasetDomainService
{
    Task DeleteAsync(string name, CancellationToken cancellationToken);
    Task<ICollection<DatasetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<double> GetAverageCliqueSize(string name, CancellationToken cancellationToken);
    Task<IDictionary<int, double>> GetAverageConnectionsAtDistances(string name, CancellationToken cancellationToken);
    Task<double> GetAverageNumberOfConnectionsPerPeerAsync(string name, CancellationToken cancellationToken);
    Task<int> GetNumberOfPeersAsync(string name, CancellationToken cancellationToken);
    Task ProcessFileAsync(Stream input, string name, CancellationToken cancellationToken);
}