using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemporalTablesDemo.Migrations
{
    public partial class EnableTemporalFeatureOnCarTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE dbo.Cars ADD PERIOD FOR SYSTEM_TIME (PeriodStart, PeriodEnd);");
            migrationBuilder.Sql("ALTER TABLE dbo.Cars SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.CarsHistory));");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // TODO undo the commands above
        }
    }
}
