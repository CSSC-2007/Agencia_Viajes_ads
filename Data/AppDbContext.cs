using Agencia_Viajes_ADS.Models;
using Agencia_Viajes_ADS.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
namespace Agencia_Viajes_ADS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<Escala> Escalas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<Rol>()
                .Property(r => r.IdRol)
                .UseIdentityByDefaultColumn();
            modelBuilder.Entity<Usuario>()
                .Property(u => u.IdUsuario)
                .UseIdentityByDefaultColumn();
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entity.GetTableName();
                if (tableName != null)
                {
                    entity.SetTableName(tableName.ToLowerInvariant());
                }
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToLowerInvariant());
                }
            }
        }
    }
}
