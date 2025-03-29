namespace Models;

public class Report
{
    public int ReportId { get; set; }
    public string ReportName { get; set; }
    public DateTime GeneratedOn { get; set; }
    public string BlobUrl { get; set; }
}