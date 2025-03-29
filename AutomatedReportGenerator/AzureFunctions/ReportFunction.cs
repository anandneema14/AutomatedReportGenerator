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

    [FunctionName("GenerateReport")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "generate-report/{id}")] HttpRequest req, int id,
        ILogger log)
    {
        log.LogInformation($"Generating report with ID: {id}");
        var result = await _reportService.GenerateReport(id);
        return new OkObjectResult(result);
    }
}