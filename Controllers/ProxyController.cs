using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dlproxy.Controllers;

[ApiController]
[Route("[controller]")]
public class ProxyController : ControllerBase
{
    private readonly ILogger<ProxyController> _logger;

    public ProxyController(ILogger<ProxyController> logger)
    {
        _logger = logger;
    }

    public static async Task DownloadBlobToStringAsync(BlobClient blobClient)
    {
        BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
        string blobContents = downloadResult.Content.ToString();
    }

    [HttpGet(Name = "GetProxy")]
    public async Task<string> Get(string containerName, string fileName)
    {
        // var containerName = "private";
        // var fileName = "test1.txt";
        var blobServiceClient = new BlobServiceClient(new Uri("https://sajkdldemo.blob.core.windows.net"), new DefaultAzureCredential());

        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        using var stream = new MemoryStream();
        await blobClient.DownloadToAsync(stream);
        stream.Position = 0;
        using var streamReader = new StreamReader(stream);
        var result = await streamReader.ReadToEndAsync();

        return result;
    }
}
