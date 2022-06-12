using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KpiV3.Domain.Migrations
{
    public partial class FixIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_files_owner_id",
                table: "files",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_files_employees_employee_id",
                table: "files",
                column: "owner_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_files_employees_employee_id",
                table: "files");

            migrationBuilder.DropIndex(
                name: "ix_files_owner_id",
                table: "files");
        }
    }
}
