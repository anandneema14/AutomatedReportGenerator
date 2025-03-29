using Azure.Storage.Blobs;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BusinessLayer;

public class ReportService : IReportService
{
    public async Task<string> GenerateReport(int reportId)
    {
        return await GenerateTaxReportPdfAsync(reportId);
    }
    
    public async Task<string> GenerateTaxReportPdfAsync(int reportId)
    {
        string blobConnectionString = Environment.GetEnvironmentVariable("BlobStorageConnectionString");
        string containerName = "reports";
        string fileName = $"report_{reportId}.pdf";

        using MemoryStream ms = new MemoryStream();
        Document doc = new Document();
        PdfWriter.GetInstance(doc, ms);
        doc.Open();
        doc.Add(new Paragraph($"Report ID: {reportId}"));
        doc.Add(new Paragraph("Generated PDF Report"));
        doc.Close();

        BlobServiceClient blobServiceClient = new BlobServiceClient(blobConnectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        ms.Position = 0;
        await blobClient.UploadAsync(ms, overwrite: true);

        return blobClient.Uri.ToString();
    }
}