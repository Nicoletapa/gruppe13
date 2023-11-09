using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Nøsted.Models;

namespace Nøsted.Data;

    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<OrdreViewModel> Ordre1 { get; set; } = default!;

        public DbSet<Sjekkliste> Sjekkliste { get; set; }
        public DbSet<Kategori> Kategori { get; set; }
        public DbSet<Sjekkpunkt> Sjekkpunkt2 { get; set; }
        public DbSet<SjekklisteSjekkpunkt> SjekklisteSjekkpunkt { get; set; }
        public DbSet<Bruker> bruker { get; set; } = default!;
    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SjekklisteSjekkpunkt>()
                .HasKey(ss => ss.SjekklisteSjekkpunktID);

            modelBuilder.Entity<SjekklisteSjekkpunkt>()
                .HasOne(j => j.sjekkliste)
                .WithMany(s => s.Sjekkpunkter)
                .HasForeignKey(j => j.SjekklisteID);


            modelBuilder.Entity<SjekklisteSjekkpunkt>()
                .HasOne(s => s.sjekkpunkt)
                .WithMany()
                .HasForeignKey(s => s.SjekkpunktID);

            modelBuilder.Entity<Sjekkliste>()
                .HasMany(s => s.Sjekkpunkter)
                .WithOne(p => p.sjekkliste);
                
            modelBuilder.Entity<Sjekkliste>()
                .HasOne(s => s.Ordre)
                .WithMany()
                .HasForeignKey(s => s.OrdreNr);

            
                
                
            modelBuilder.Entity<Bruker>().HasDiscriminator().HasValue("");
  

            // Define primary key for Sjekkpunkt entity
            modelBuilder.Entity<Sjekkpunkt>().HasKey(p => p.SjekkpunktID);

            base.OnModelCreating(modelBuilder);
        }
        
    
        
     
    }
