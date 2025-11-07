using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS2.Migrations
{
    /// <inheritdoc />
    public partial class add_PositionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Employees");

            migrationBuilder.AddColumn<long>(
                name: "PositionId",
                table: "Employees",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lookupId",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_lookupId",
                table: "Employees",
                column: "lookupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Lookups_lookupId",
                table: "Employees",
                column: "lookupId",
                principalTable: "Lookups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Lookups_lookupId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_lookupId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "lookupId",
                table: "Employees");

            migrationBuilder.AddColumn<long>(
                name: "Position",
                table: "Employees",
                type: "bigint",
                maxLength: 50,
                nullable: false,
                defaultValue: 0L);
        }
    }
}
