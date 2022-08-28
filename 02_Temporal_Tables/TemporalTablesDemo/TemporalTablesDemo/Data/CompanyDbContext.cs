using Microsoft.EntityFrameworkCore;
using TemporalTablesDemo.Data.Models;

namespace TemporalTablesDemo.Data
{
    public class CompanyDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Employee>().ToTable("Employees", e => e.IsTemporal());
        }
    }
}
