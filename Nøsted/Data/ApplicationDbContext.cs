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

       
        public DbSet<Kategori> Kategori { get; set; }
        public DbSet<Sjekkpunkt> Sjekkpunkt { get; set; }
        public DbSet<SjekklisteSjekkpunkt> SjekklisteSjekkpunkt { get; set; }
        public DbSet<Bruker> bruker { get; set; } = default!;
    

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

            
                
          
            
                
                
            modelBuilder.Entity<Bruker>().HasDiscriminator().HasValue("");
  

            // Define primary key for Sjekkpunkt entity
            modelBuilder.Entity<Sjekkpunkt>().HasKey(p => p.SjekkpunktID);

            base.OnModelCreating(modelBuilder);
        }
        
    
        
     
    }
