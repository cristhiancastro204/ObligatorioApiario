using Microsoft.EntityFrameworkCore;
using ObligatorioApiario.Models;

namespace ObligatorioApiario.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Apiario> Apiarios { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Colmena> Colmenas { get; set; }
        public DbSet<Cosecha> Cosechas { get; set; }
        public DbSet<CosechaColmena> CosechasColmenas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CosechaColmena>()
                .Property(c => c.CantidadKg)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CosechaColmena>()
                .HasOne(cc => cc.Colmena)
                .WithMany(c => c.HistorialCosechas)
                .HasForeignKey(cc => cc.ColmenaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
