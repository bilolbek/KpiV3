using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KpiV3.Domain.Migrations
{
    public partial class RestrictSubmission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_submissions_requirement_id",
                table: "submissions");

            migrationBuilder.CreateIndex(
                name: "ix_submissions_requirement_id_employee_id",
                table: "submissions",
                columns: new[] { "requirement_id", "employee_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_submissions_requirement_id_employee_id",
                table: "submissions");

            migrationBuilder.CreateIndex(
                name: "ix_submissions_requirement_id",
                table: "submissions",
                column: "requirement_id");
        }
    }
}
