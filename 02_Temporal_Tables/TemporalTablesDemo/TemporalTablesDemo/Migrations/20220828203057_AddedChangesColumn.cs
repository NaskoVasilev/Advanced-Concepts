using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemporalTablesDemo.Migrations
{
    public partial class AddedChangesColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Changes",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Changes",
                table: "Employees")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EmployeesHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
