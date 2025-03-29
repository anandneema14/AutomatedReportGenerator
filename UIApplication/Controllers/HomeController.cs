using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UIApplication.Models;

namespace UIApplication.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetReportLink(int reportId)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"https://localhost:5001/api/reports/{reportId}");

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            ViewBag.ReportLink = data;
        }
        else
        {
            ViewBag.Message = "Error retrieving report link.";
        }
        return View("Index");
    }
}