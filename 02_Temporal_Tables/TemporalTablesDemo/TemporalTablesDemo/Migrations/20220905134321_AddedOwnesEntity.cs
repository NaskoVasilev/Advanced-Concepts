using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemporalTablesDemo.Migrations
{
    public partial class AddedOwnesEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "PeriodEnd",
            //    table: "Employees")
            //    .Annotation("SqlServer:IsTemporal", true)
            //    .Annotation("SqlServer:TemporalHistoryTableName", "EmployeesHistory")
            //    .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            //    .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            //    .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            //migrationBuilder.DropColumn(
            //    name: "PeriodStart",
            //    table: "Employees")
            //    .Annotation("SqlServer:IsTemporal", true)
            //    .Annotation("SqlServer:TemporalHistoryTableName", "EmployeesHistory")
            //    .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            //    .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            //    .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            //migrationBuilder.AlterTable(
            //    name: "Employees")
            //    .OldAnnotation("SqlServer:IsTemporal", true)
            //    .OldAnnotation("SqlServer:TemporalHistoryTableName", "EmployeesHistory")
            //    .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            //    .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            //    .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<DateTime>(
                name: "Birth_Date",
                table: "Employees",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birth_Date",
                table: "Employees");

            //migrationBuilder.AlterTable(
            //    name: "Employees")
            //    .Annotation("SqlServer:IsTemporal", true)
            //    .Annotation("SqlServer:TemporalHistoryTableName", "EmployeesHistory")
            //    .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            //    .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            //    .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "PeriodEnd",
            //    table: "Employees",
            //    type: "datetime2",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
            //    .Annotation("SqlServer:IsTemporal", true)
            //    .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            //    .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "PeriodStart",
            //    table: "Employees",
            //    type: "datetime2",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
            //    .Annotation("SqlServer:IsTemporal", true)
            //    .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            //    .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
