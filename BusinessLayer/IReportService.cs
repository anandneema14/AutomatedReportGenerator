namespace BusinessLayer;

public interface IReportService
{
    Task<string> GenerateReport(int reportId);
}