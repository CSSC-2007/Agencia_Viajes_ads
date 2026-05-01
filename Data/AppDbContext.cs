using Microsoft.EntityFrameworkCore;
using Agencia_Viajes_ADS.Models;

namespace Agencia_Viajes_ADS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("public");

            // Configure identity columns for PostgreSQL
            modelBuilder.Entity<Rol>()
                .Property(r => r.IdRol)
                .UseIdentityByDefaultColumn();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.IdUsuario)
                .UseIdentityByDefaultColumn();

            // Force lowercase names for PostgreSQL compatibility
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Replace table names
                var tableName = entity.GetTableName();
                if (tableName != null)
                {
                    entity.SetTableName(tableName.ToLowerInvariant());
                }

                // Replace column names
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToLowerInvariant());
                }
            }
        }
    }
}
