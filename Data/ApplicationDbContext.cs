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

            // POSTGRESQL FIX: Convert all DateTime properties to UTC before saving, 
            // and back from UTC when reading, to avoid Npgsql timestamp strictness errors.
            var dateTimeConverter = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableDateTimeConverter = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.IsKeyless) continue;

                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                        property.SetValueConverter(dateTimeConverter);
                    else if (property.ClrType == typeof(DateTime?))
                        property.SetValueConverter(nullableDateTimeConverter);
                }
            }
        }
    }
}
