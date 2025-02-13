using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi
{
    public class DbEntity : DbContext
    {
        public DbSet<IpRow> IpDetailsTable { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "KPET-LAPTOP\\SQLEXPRESS",
                IntegratedSecurity = true,
                InitialCatalog = "IpDetails",
                TrustServerCertificate = true
            };

            var connectionString = builder.ConnectionString;

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IpRow>()
                .HasKey(i => i.IP);
        }

        [Table("IpDetails")]
        public class IpRow
        {
            [Key]
            public string IP { get; set; } = null!;
            public string City { get; set; } = null!;
            public string Country { get; set; } = null!;
            public string Continent { get; set; } = null!;
            public double Lat { get; set; }
            public double Lon { get; set; }
        }
    }
}

