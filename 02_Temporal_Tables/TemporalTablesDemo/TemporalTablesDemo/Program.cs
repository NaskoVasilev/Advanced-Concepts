using System;
using TemporalTablesDemo.Data.Models;
using TemporalTablesDemo.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TemporalTablesDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // SeedData();
            // UpdateEmployee("Leanney", "Atanas");
            // DeleteEmployee("Ervint");
            // var employeeId = UpdateEmployee("Atanas", "Nasko");

            //if (employeeId.HasValue)
            //{
            //    PrintEmployeeHistory(employeeId.Value);
            //}

            // PrintEmployeeHistory(1);

            RestoreDeletedEmployee(2);
        }

        private void SeedData()
        {
            Console.WriteLine("Initializing...");
            Employee hrEmp1 = new()
            {
                FirstName = "Leanney",
                LastName = "Graham",
                Department = "HR"
            };
            Employee hrEmp2 = new()
            {
                FirstName = "Ervint",
                LastName = "Howell",
                Department = "HR"
            };
            Employee legalEmp1 = new()
            {
                FirstName = "Clementine",
                LastName = "Baucho",
                Department = "Legal"
            };
            Employee itEmp1 = new()
            {
                FirstName = "Patriciari",
                LastName = "Lebsack",
                Department = "IT"
            };
            Employee itEmp2 = new()
            {
                FirstName = "Chelst",
                LastName = "Dietrich",
                Department = "IT"
            };
            Employee itEmp3 = new()
            {
                FirstName = "Kurt",
                LastName = "Weissnat",
                Department = "IT"
            };

            using
            var dbContext = new CompanyDbContext();
            dbContext.AddRange(hrEmp1, hrEmp2, legalEmp1, itEmp1, itEmp2, itEmp3);
            dbContext.SaveChanges();
            Console.WriteLine("Initialization done !");
        }

        private static int? UpdateEmployee(string firstName, string newName)
        {
            using
            var dbContext = new CompanyDbContext();
            var employee = dbContext.Employees.FirstOrDefault(x => x.FirstName == firstName);
            if (employee != null)
            {
                employee.FirstName = newName;
                dbContext.SaveChanges();
                Console.WriteLine($"Employee: {employee.Id}, {employee.FirstName} {employee.LastName} was successfully updated");
            }

            return employee?.Id ?? null;
        }

        private static void DeleteEmployee(string firstName)
        {
            using
            var dbContext = new CompanyDbContext();
            var employee = dbContext.Employees.FirstOrDefault(x => x.FirstName == firstName);
            if (employee != null)
            {
                dbContext.Remove(employee);
                dbContext.SaveChanges();
                Console.WriteLine($"Employee: {employee.Id}, {employee.FirstName} {employee.LastName} was successfully deleted");
            }
        }

        private static void PrintEmployeeHistory(int id)
        {
            using
            var context = new CompanyDbContext();
            context.Employees.TemporalAll().Where(e => e.Id == id)
                .OrderByDescending(e => EF.Property<DateTime>(e, "PeriodStart"))
                .Select(e => new
                {
                    Employee = e,
                    PeriodStart = EF.Property<DateTime>(e, "PeriodStart"),
                    PeriodEnd = EF.Property<DateTime>(e, "PeriodEnd")
                })
                .ToList()
                .ForEach(x =>
                {
                    Console.WriteLine(x.ToString());
                    Console.WriteLine($"{x.PeriodStart} - {x.PeriodEnd}");
                    Console.WriteLine(new string('-', 100));
                });
        }

        public static void RestoreDeletedEmployee(int id)
        {
            using
            var dbContext = new CompanyDbContext();
            var deleteTimestamp = dbContext.Employees
                .TemporalAll()
                .Where(e => e.Id == id)
                .OrderBy(e => EF.Property<DateTime>(e, "PeriodEnd"))
                .Select(e => EF.Property<DateTime>(e, "PeriodEnd"))
                .Last();

            var deletedEmployee = dbContext.Employees
                .TemporalAsOf(deleteTimestamp.AddMilliseconds(-1))
                .FirstOrDefault(emp => emp.Id == id);

            if (deletedEmployee != null)
            {
                dbContext.Add(deletedEmployee);
                SetIdentityInsert<Employee>(true);
                dbContext.SaveChanges();
                SetIdentityInsert<Employee>(false);

                Console.WriteLine($"The empoyee: {deletedEmployee.FirstName} {deletedEmployee.LastName} was succesfully restored!");
            }
        }

        private static void SetIdentityInsert<T>(bool value)
        {
            using
            var dbContext = new CompanyDbContext();
            dbContext.Database.OpenConnection();
            var entityType = dbContext.Model.FindEntityType(typeof(T));
            if (value)
            {
                dbContext.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} ON;");
            }
            else
            {
                dbContext.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} OFF;");
            }
        }
    }
}