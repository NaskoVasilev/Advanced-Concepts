using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemporalTablesDemo.Migrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<DateTime>(
            //    name: "PeriodEnd",
            //    table: "Employees",
            //    type: "datetime2",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "PeriodStart",
            //    table: "Employees",
            //    type: "datetime2",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "PeriodEnd",
            //    table: "Employees");

            //migrationBuilder.DropColumn(
            //    name: "PeriodStart",
            //    table: "Employees");
        }
    }
}
