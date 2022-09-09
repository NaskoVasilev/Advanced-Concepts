using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using TemporalTablesDemo.Data.Models;
using System.Collections.Generic;
using System.Text.Json;
using TemporalTablesDemo.Data.Common;
using TemporalTablesDemo.Data.Interceptors;

namespace TemporalTablesDemo.Data
{
    public class CompanyDbContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Car> Cars { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new HistoryQueryInterceptor());
            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // TODO get all models implementing ITemporalTable and register them automatically
            builder.Entity<Employee>().ToTable("Employees", e => e.IsTemporal());
            builder.Entity<Company>().ToTable("Companies", e => e.IsTemporal());

            builder.Entity<Employee>().Property<string>(ModelConstants.ChangesColumnName);
            builder.Entity<Company>().Property<string>(ModelConstants.ChangesColumnName);

            builder.Entity<Car>().OwnsOne(x => x.Engine);

            builder.Entity<Car>().Property<DateTime>(ModelConstants.PeriodStartColumnName)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime2")
                .HasColumnName(ModelConstants.PeriodStartColumnName);

            builder.Entity<Car>().Property<DateTime>(ModelConstants.PeriodEndColumnName)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValue(DateTime.MaxValue)
                .HasColumnType("datetime2")
                .HasColumnName(ModelConstants.PeriodEndColumnName);

            builder.Entity<Car>().Property<string>(ModelConstants.ChangesColumnName);
        }

        public override int SaveChanges()
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Modified && p.Entity is ITemporalEntity)
                .ToList();
            var now = DateTime.UtcNow;

            foreach (var change in modifiedEntities)
            {
                var changes = new List<PropertyChange>();
                var entityName = change.Entity.GetType().Name;

                foreach (var prop in change.OriginalValues.Properties)
                {
                    var originalValue = change.OriginalValues[prop];
                    var currentValue = change.CurrentValues[prop];
                    if (originalValue?.ToString() != currentValue?.ToString()) 
                    {
                        changes.Add(new PropertyChange
                        {
                            CurrentValue = currentValue,
                            PreviousValue = originalValue,
                            Name = prop.Name
                        });
                    }
                }

                change.Property(ModelConstants.ChangesColumnName).CurrentValue = JsonSerializer.Serialize(changes);
                Console.WriteLine($"{entityName} -> {JsonSerializer.Serialize(changes)}");
            }

            return base.SaveChanges();
        }
    }
}
