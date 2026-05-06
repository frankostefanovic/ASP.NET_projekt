using Microsoft.EntityFrameworkCore;
using Lab2.RezervacijeProstora.Models;

namespace Lab2.RezervacijeProstora
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet za sve modele
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<ProstorZaProbu> Prostori { get; set; }
        public DbSet<Rezervacija> Rezervacije { get; set; }
        public DbSet<Oprema> Oprema { get; set; }
        public DbSet<Placanje> Placanja { get; set; }
        public DbSet<Recenzija> Recenzije { get; set; }
        public DbSet<Vlasnik> Vlasnici { get; set; }
        public DbSet<Lokacija> Lokacije { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProstorZaProbu>()
                .HasOne(p => p.Lokacija)
                .WithMany(l => l.Prostori)
                .HasForeignKey(p => p.LokacijaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProstorZaProbu>()
                .HasOne(p => p.Vlasnik)
                .WithMany(v => v.Prostori)
                .HasForeignKey(p => p.VlasnikId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rezervacija>()
                .HasOne(r => r.Korisnik)
                .WithMany(k => k.Rezervacije)
                .HasForeignKey(r => r.KorisnikId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rezervacija>()
                .HasOne(r => r.Prostor)
                .WithMany(p => p.Rezervacije)
                .HasForeignKey(r => r.ProstorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rezervacija>()
                .HasOne(r => r.Placanje)
                .WithOne(p => p.Rezervacija)
                .HasForeignKey<Placanje>(p => p.RezervacijaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Recenzija>()
                .HasOne(r => r.Korisnik)
                .WithMany(k => k.Recenzije)
                .HasForeignKey(r => r.KorisnikId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Recenzija>()
                .HasOne(r => r.Prostor)
                .WithMany(p => p.Recenzije)
                .HasForeignKey(r => r.ProstorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProstorZaProbu>()
                .HasMany(p => p.Oprema)
                .WithMany(o => o.Prostori)
                .UsingEntity(j => j.ToTable("ProstorOprema"));
        }
    }
}
