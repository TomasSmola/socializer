using Microsoft.AspNetCore.Mvc;
using socializer.Core.Datasets;

namespace socializer.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class DatasetController(IDatasetDomainService datasetDomainService) : ControllerBase
{
    [HttpPost("import")]
    public async Task<IActionResult> ImportAsync(IFormFile datasetFile, [FromForm] string datasetName, CancellationToken cancellationToken)
    {
        var fileName = datasetName;
        var fileStream = datasetFile.OpenReadStream();

        await datasetDomainService.ProcessFileAsync(fileStream, fileName, cancellationToken);

        return Ok("File Was Processed Sucessfully!");
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var datasets = await datasetDomainService.GetAllAsync(cancellationToken);

        return Ok(datasets);
    }

    [HttpGet("average-number-of-connections")]
    public async Task<IActionResult> GetAverageNumberOfConnectionsPerPeerAsync([FromQuery] string name, CancellationToken cancellationToken)
    {
        var averageNumberOfConnectionsPerPeer = await datasetDomainService.GetAverageNumberOfConnectionsPerPeerAsync(name, cancellationToken);

        return Ok(averageNumberOfConnectionsPerPeer);
    }

    [HttpGet("number-of-peers")]
    public async Task<IActionResult> GetNumberOfPeersAsync([FromQuery] string name, CancellationToken cancellationToken)
    {
        var numberOfPeers = await datasetDomainService.GetNumberOfPeersAsync(name, cancellationToken);

        return Ok(numberOfPeers);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync([FromQuery] string name, CancellationToken cancellationToken)
    {
        await datasetDomainService.DeleteAsync(name, cancellationToken);

        return Ok("Dataset Deleted Successfully!");
    }

    [HttpGet("average-connections-at-distances")]
    public async Task<IActionResult> GetAverageConnectionsAtDistances([FromQuery] string name, CancellationToken cancellationToken)
    {
        var averageConnectionsAtDistances = await datasetDomainService.GetAverageConnectionsAtDistances(name, cancellationToken);

        return Ok(averageConnectionsAtDistances);
    }

    [HttpGet("average-clique-size")]
    public async Task<IActionResult> GetAverageCliqueSize([FromQuery] string name, CancellationToken cancellationToken)
    {
        var averageCliqueSize = await datasetDomainService.GetAverageCliqueSize(name, cancellationToken);

        return Ok(averageCliqueSize);
    }
}
