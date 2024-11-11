using Microsoft.EntityFrameworkCore;
using socializer.Application.Dtos;
using socializer.Core.Datasets.Analyzers;
using socializer.Data.Domain.Entities;
using socializer.Data.Domain.Interfaces;

namespace socializer.Core.Datasets;

public class DatasetDomainService(
    IRepository<Dataset> datasetRepository,
    IRepository<SocialConnection> socialConnectionRepository,
    INumberOfPeersAnalyzer numberOfPeersAnalyzer,
    IAverageNumberOfConnectionsPerPeerAnalyzer averageNumberOfConnectionsPerPeerAnalyzer,
    IAverageConnectionsAtDistancesDatasetAnalyzer averageConnectionsAtDistancesDatasetAnalyzer,
    IAverageCliqueSizeDatasetAnalyzer averageCliqueSizeDatasetAnalyzer) : IDatasetDomainService
{
    public async Task<ICollection<DatasetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await datasetRepository
            .GetAll()
            .Select(d => new DatasetDto(d.Id, d.Name, d.ImportTimestamp))
            .ToListAsync(cancellationToken);
    }

    public async Task ProcessFileAsync(Stream input, string name, CancellationToken cancellationToken)
    {
        var timestamp = DateTimeOffset.UtcNow;

        var dataset = await datasetRepository.CreateAsync(new Dataset
        {
            Name = name,
            ImportTimestamp = timestamp
        },
        cancellationToken);

        await foreach (var (peer1Id, peer2Id) in DatasetParser.Parse(input))
        {
           await socialConnectionRepository.CreateAsync(new SocialConnection
           {
               DatasetId = dataset.Id,
               Peer1Id = peer1Id,
               Peer2Id = peer2Id
           },
           cancellationToken);
        }
    }

    public async Task<int> GetNumberOfPeersAsync(string name, CancellationToken cancellationToken)
    {
        var datasetId = await datasetRepository
            .GetAll()
            .Where(d => d.Name == name)
            .Select(d => d.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var connections = await socialConnectionRepository
            .GetAll()
            .Where(s => s.DatasetId == datasetId)
            .ToListAsync(cancellationToken);

        return numberOfPeersAnalyzer.Calculate(connections);
    }

    public async Task<double> GetAverageNumberOfConnectionsPerPeerAsync(string name, CancellationToken cancellationToken)
    {
        var datasetId = await datasetRepository
            .GetAll()
            .Where(d => d.Name == name)
            .Select(d => d.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var peerConnections = new Dictionary<int, HashSet<int>>();

        var connections = await socialConnectionRepository
            .GetAll()
            .Where(s => s.DatasetId == datasetId)
            .ToListAsync();

        return averageNumberOfConnectionsPerPeerAnalyzer.Calculate(connections);
    }

    public async Task DeleteAsync(string name, CancellationToken cancellationToken)
    {
        var datasetId = await datasetRepository
            .GetAll()
            .Where(d => d.Name == name)
            .Select(d => d.Id)
            .FirstOrDefaultAsync(cancellationToken);

        await socialConnectionRepository
            .DeleteAllAsync(s => s.DatasetId == datasetId, cancellationToken);
        await datasetRepository
            .DeleteAllAsync(d => d.Id == datasetId, cancellationToken);
    }

    public async Task<IDictionary<int, double>> GetAverageConnectionsAtDistances(string name, CancellationToken cancellationToken)
    {
        var datasetId = await datasetRepository
            .GetAll()
            .Where(d => d.Name == name)
            .Select(d => d.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var connections = await socialConnectionRepository
            .GetAll()
            .Where(s => s.DatasetId == datasetId)
            .ToListAsync(cancellationToken);

        return averageConnectionsAtDistancesDatasetAnalyzer
            .Calculate(connections);
    }

    public async Task<double> GetAverageCliqueSize(string name, CancellationToken cancellationToken)
    {
        var datasetId = await datasetRepository
            .GetAll()
            .Where(d => d.Name == name)
            .Select(d => d.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var connections = await socialConnectionRepository
            .GetAll()
            .Where(s => s.DatasetId == datasetId)
            .ToListAsync(cancellationToken);

        return averageCliqueSizeDatasetAnalyzer
            .Calculate(connections);
    }
}
