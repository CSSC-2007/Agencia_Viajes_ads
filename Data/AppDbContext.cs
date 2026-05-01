using Microsoft.EntityFrameworkCore;

namespace Agencia_Viajes_ADS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
    }
}
