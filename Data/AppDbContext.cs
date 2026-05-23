using Agencia_Viajes_ADS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        public DbSet<MetodoPago> MetodosPago { get; set; }

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

            modelBuilder.Entity<Escala>()
                .Property(e => e.IdEscala)
                .UseIdentityByDefaultColumn();

            modelBuilder.Entity<MetodoPago>()
                .Property(m => m.IdMetodo)
                .UseIdentityByDefaultColumn();

            // ==================================
            // CONVERSIÓN GLOBAL DE DATETIME A UTC
            // ==================================

            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc
                    ? v
                    : DateTime.SpecifyKind(v, DateTimeKind.Utc),

                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();

                if (tableName != null)
                {
                    entityType.SetTableName(tableName.ToLowerInvariant());
                }

                foreach (var property in entityType.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToLowerInvariant());

                    // Aplicar UTC automáticamente
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                }
            }
        }
    }
}