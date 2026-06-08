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
    }
}
