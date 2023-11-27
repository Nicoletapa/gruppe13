using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Nøsted.Models;
using Nøsted.Entities;

namespace Nøsted.Data;
    
    /// <summary>
    /// Provides the database context for the application, including identity and application-specific entities.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Represents the orders in the database.
        public DbSet<OrdreEntity> Ordre1 { get; set; } = default!;

        // Represents categories in the database.
        public DbSet<Kategori> Kategori { get; set; }

        // Represents checkpoints in the database.
        public DbSet<Sjekkpunkt> Sjekkpunkt { get; set; }

        // Represents checklist checkpoints in the database.
        public DbSet<SjekklisteSjekkpunkt> SjekklisteSjekkpunkt { get; set; }

        /// <summary>
        /// Configures entity relationships and database schema settings.
        /// </summary>
        /// <param name="modelBuilder">Provides an API for configuring the model that maps to the database.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sjekkpunkt>()
                .HasOne(s => s.Kategori)
                .WithMany()
                .HasForeignKey(s => s.KategoriID);

            modelBuilder.Entity<SjekklisteSjekkpunkt>()
                .HasOne(ss => ss.sjekkpunkt)
                .WithMany()
                .HasForeignKey(ss => ss.SjekkpunktID);
        
           

            modelBuilder.Entity<Sjekkpunkt>().HasKey(p => p.SjekkpunktID);

            base.OnModelCreating(modelBuilder);
        }
        
    
     
    }
