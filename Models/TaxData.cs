namespace Models;

public class TaxData
{
    public int TaxId { get; set; }
    public int ReportId { get; set; }
    public decimal GrossIncome { get; set; }
    public decimal Deductions { get; set; }
    public decimal TaxRate { get; set; }
    public decimal NetIncome { get; set; }
}