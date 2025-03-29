using Microsoft.AspNetCore.Mvc;

namespace AutomatedReportGenerator.Controllers;

[ApiController]
public class ReportController : Controller
{
    [HttpPost("generate/{reportId}")]
    public async Task<IActionResult> GenerateReport(int reportId)
    {
        try
        {
            string reportUrl = $"https://yourstorage.blob.core.windows.net/reports/{reportId}.pdf";
            return Ok(new { Message = "Report generated successfully", ReportUrl = reportUrl });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"Error generating report: {ex.Message}" });
        }
    }
    
    [HttpGet("{reportId}")]
    public IActionResult GetReport(int reportId)
    {
        string reportUrl = $"https://yourstorage.blob.core.windows.net/reports/{reportId}.pdf";
        return Ok(new { ReportUrl = reportUrl });
    }
}