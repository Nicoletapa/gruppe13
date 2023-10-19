using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nøsted.Models;

namespace Nøsted.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<OrdreViewModel> Ordre { get; set; } = default!;
    public DbSet<ArbeidsDokumentViewModel> ArbeidsDokument2 { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<ArbeidsDokumentViewModel>()
            .HasOne(ad => ad.Ordre)
            .WithMany()
            .HasForeignKey(ad => ad.OrdreID);
        
        base.OnModelCreating(modelBuilder);
    }

}