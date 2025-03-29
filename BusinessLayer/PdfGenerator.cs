using System.Data.SqlClient;
using Azure.Storage.Blobs;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BusinessLayer;

public class PdfGenerator
{
    private readonly string _connectionString;
    private readonly string _blobContainerName;
    private readonly string _storageConnectionString;

    public PdfGenerator(string connectionString, string blobContainerName, string storageConnectionString)
    {
        _connectionString = connectionString;
        _blobContainerName = blobContainerName;
        _storageConnectionString = storageConnectionString;
    }

    public async Task GenerateTaxReportPdfAsync(int reportId)
    {
        var taxData = GetTaxData(reportId);

        if (taxData == null)
        {
            Console.WriteLine("No tax data found for the given report ID.");
            return;
        }

        using (var memoryStream = new MemoryStream())
        {
            Document document = new Document();
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            // PDF Content
            document.Add(new Paragraph("Tax Report"));
            document.Add(new Paragraph("-----------------------"));
            document.Add(new Paragraph($"Gross Income: ${taxData.GrossIncome:F2}"));
            document.Add(new Paragraph($"Deductions: ${taxData.Deductions:F2}"));
            document.Add(new Paragraph($"Tax Rate: {taxData.TaxRate}%"));
            document.Add(new Paragraph($"Calculated Tax: ${taxData.CalculatedTax:F2}"));
            document.Add(new Paragraph($"Net Income: ${taxData.NetIncome:F2}"));
            document.Close();

            await UploadPdfToBlobStorageAsync(memoryStream, $"tax_report_{reportId}.pdf");
        }

        Console.WriteLine("Tax PDF generated and uploaded successfully.");
    }

    private (decimal GrossIncome, decimal Deductions, decimal TaxRate, decimal CalculatedTax, decimal NetIncome)? GetTaxData(int reportId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"
                SELECT GrossIncome, Deductions, TaxRate,
                       (GrossIncome - Deductions) * (TaxRate / 100) AS CalculatedTax,
                       GrossIncome - ((GrossIncome - Deductions) * (TaxRate / 100)) AS NetIncome
                FROM TaxData
                WHERE ReportID = @ReportID";
            
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ReportID", reportId);
            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (
                        GrossIncome: reader.GetDecimal(0),
                        Deductions: reader.GetDecimal(1),
                        TaxRate: reader.GetDecimal(2),
                        CalculatedTax: reader.GetDecimal(3),
                        NetIncome: reader.GetDecimal(4)
                    );
                }
            }
        }
        return null;
    }

    private async Task UploadPdfToBlobStorageAsync(MemoryStream stream, string fileName)
    {
        stream.Position = 0;
        BlobServiceClient blobServiceClient = new BlobServiceClient(_storageConnectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_blobContainerName);
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(stream, overwrite: true);
    }
}