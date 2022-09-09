using System;
using TemporalTablesDemo.Data.Models;
using TemporalTablesDemo.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TemporalTablesDemo.Data.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using TemporalTablesDemo.Data.Models.Enums;
using TemporalTablesDemo.Services.Models;

namespace TemporalTablesDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //SeedData();
            //UpdateEmployee("Kurt", "Edited1");
            //DeleteEmployee("Ervint");
            //var employeeId = UpdateEmployee("Edited1", "Edited2");

            //if (employeeId.HasValue)
            //{
            // PrintEmployeeHistory(employeeId.Value);
            //}


            // RestoreDeletedEmployee(2);

            //var companyId = CreateCompany();
            //UpdateCompany(companyId, 1);
            //UpdateCompany(companyId, 2);

            //using
            //var context = new CompanyDbContext();
            //var historyService = new HistoryService(context);
            //var result = historyService.GetHistoryById(2).ToList();
            //Console.WriteLine(JsonSerializer.Serialize(result));

            //var carId = CreateCar();
            //UpdateCar(carId, 1);
            //UpdateCar(carId, 2);

            //using
            //var context = new CompanyDbContext();
            //var historyService = new HistoryService(context);
            //var result = historyService.GetHistoryById<Car>(carId).ToList();
            //Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions
            //{
            //    WriteIndented = true,
            //}));
        }

        private static int CreateCar()
        {
            var car = new Car
            {
                Make = "Lexus",
                Model = "IS 220",
                Engine = new Engine
                {
                    Horsepower = 177,
                    Capacity = 2.2,
                    Type = EngineType.Diesel,
                }
            };

            using
            var context = new CompanyDbContext();
            context.Add(car);
            context.SaveChanges();
            return car.Id;
        }

        private static void UpdateCar(int id, int version)
        {
            var rnd = new Random();
            using
            var context = new CompanyDbContext();
            var car = context.Cars
                .FirstOrDefault(x => x.Id == id);

            car.Model = MarkAsEdited(car.Model, version);
            car.Make = MarkAsEdited(car.Make, version);
            car.Engine = new Engine()
            {
                Capacity = rnd.Next(2, 5),
                Horsepower = rnd.Next(100, 500),
                Type = (EngineType)rnd.Next(1, 4),
            };

            context.Update(car);
            context.SaveChanges();
        }

        private static int CreateCompany()
        {
            var company = new Company
            {
                Name = "Company 1",
                Employees = Enumerable.Range(1, 3)
                    .Select(x => new Employee
                    {
                        FirstName = "FN1-" + x,
                        LastName = "LN1-" + x,
                        Department = "Department1",
                    })
                    .ToList()
            };

            using
            var context = new CompanyDbContext();
            context.Add(company);
            context.SaveChanges();
            return company.Id;
        }

        private static void UpdateCompany(int id, int version)
        {
            using
            var context = new CompanyDbContext();
            var company = context.Companies
                .Include(x => x.Employees)
                .FirstOrDefault(x => x.Id == id);

            company.Name = MarkAsEdited(company.Name, version);
            foreach (var employee in company.Employees)
            {
                employee.LastName = MarkAsEdited(employee.LastName, version);
                employee.FirstName = MarkAsEdited(employee.FirstName, version);
            }

            context.Update(company);
            context.SaveChanges();
        }

        private static void SeedData()
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
                var change = new PropertyChange(nameof(Employee.FirstName), employee.FirstName, newName);
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
                    PeriodEnd = EF.Property<DateTime>(e, "PeriodEnd"),
                    Changes = EF.Property<string>(e, ModelConstants.ChangesColumnName),
                })
                .ToList()
                .ForEach(x =>
                {
                    Console.WriteLine(x.Employee.ToString());
                    Console.WriteLine($"Chnages: {x.Changes}");
                    // Console.WriteLine($"{x.PeriodStart} - {x.PeriodEnd}");
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

        private static string MarkAsEdited(string value, int version) => $"{value}-E-{version}";
    }
}