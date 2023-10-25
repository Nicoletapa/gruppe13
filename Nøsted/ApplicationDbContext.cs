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
    public DbSet<OrdreViewModel> Ordre1 { get; set; } = default!;


    public DbSet<SjekklisteViewModel> SjekklisteViewModel1 { get; set; } = default!;
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-one relationships for each related class
            /*modelBuilder.Entity<SjekklisteViewModel>()
                .HasOne(vm => vm.SjekklisteMekanisk)
                .WithOne(m => m.SjekklisteViewModel)
                .HasForeignKey<SjekklisteMekanisk>(m => m.SjekklisteViewModelID);

            modelBuilder.Entity<SjekklisteViewModel>()
                .HasOne(vm => vm.SjekklisteHydraulisk)
                .WithOne(h => h.SjekklisteViewModel)
                .HasForeignKey<SjekklisteHydraulisk>(h => h.SjekklisteViewModelID);

            modelBuilder.Entity<SjekklisteViewModel>()
                .HasOne(vm => vm.SjekklisteElektro)
                .WithOne(e => e.SjekklisteViewModel)
                .HasForeignKey<SjekklisteElektro>(e => e.SjekklisteViewModelID);

            modelBuilder.Entity<SjekklisteViewModel>()
                .HasOne(vm => vm.SjekklisteTrykkSettinger)
                .WithOne(ts => ts.SjekklisteViewModel)
                .HasForeignKey<SjekklisteTrykkSettinger>(ts => ts.SjekklisteViewModelID);

            modelBuilder.Entity<SjekklisteViewModel>()
                .HasOne(vm => vm.SjekklisteFunksjonsTest)
                .WithOne(ft => ft.SjekklisteViewModel)
                .HasForeignKey<SjekklisteFunksjonsTest>(ft => ft.SjekklisteViewModelID);

            modelBuilder.Entity<SjekklisteViewModel>()
                .HasOne(vm => vm.SjekklisteKommentarer)
                .WithOne(k => k.SjekklisteViewModel)
                .HasForeignKey<SjekklisteKommentarer>(k => k.SjekklisteViewModelID);*/
            base.OnModelCreating(modelBuilder);
        }

     /*
     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
    //     modelBuilder.Entity<SjekklisteViewModel>().HasNoKey();
    modelBuilder.Entity<SjekklisteViewModel>()
        .HasOne(vm => vm.SjekklisteFunksjonsTest) // One SjekklisteViewModel has one SjekklisteFunksjonsTest
        .WithOne(ft => ft.SjekklisteViewModel);
       
    
     }
    //    modelBuilder.Entity<ArbeidsDokumentViewModel>()
    //        .HasOne(ad => ad.Ordre)
    //      .WithMany()
    //    .HasForeignKey(ad => ad.OrdreID);
    */

    
     

}