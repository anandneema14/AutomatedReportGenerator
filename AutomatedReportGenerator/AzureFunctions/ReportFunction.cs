using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace AutomatedReportGenerator.AzureFunctions;

public class ReportFunction
{
    private readonly IReportService _reportService;

    public ReportFunction(IReportService reportService)
    {
        _reportService = reportService;
    }

    [FunctionName("GenerateReport")]
    public Task<IActionResult> Run(int id, ILogger log)
    {
        return Run(null, id, log);
    }

    [FunctionName("GenerateTaxReportPdf")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "generate-tax-pdf/{reportId}")] HttpRequest req,
        int reportId,
        ILogger log)
    {
        var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
        var blobContainerName = Environment.GetEnvironmentVariable("BlobContainerName");
        var storageConnectionString = Environment.GetEnvironmentVariable("AzureStorageConnectionString");

        var pdfGenerator = new PdfGenerator(connectionString, blobContainerName, storageConnectionString);
        await pdfGenerator.GenerateTaxReportPdfAsync(reportId);

        return new OkObjectResult($"Tax PDF generated for Report ID: {reportId}");
    }
}