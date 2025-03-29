using Microsoft.EntityFrameworkCore;

namespace Models;

public class ReportContext : DbContext
{
    public ReportContext(DbContextOptions<ReportContext> options) : base(options) { }
    public DbSet<Report> Reports { get; set; }
    public DbSet<TaxData> TaxData { get; set; }
}