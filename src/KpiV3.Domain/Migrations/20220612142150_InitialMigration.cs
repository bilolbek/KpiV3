using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KpiV3.Domain.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "comment_blocks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comment_blocks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    content_type = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    length = table.Column<long>(type: "bigint", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_files", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "indicators",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    description = table.Column<string>(type: "character varying(32768)", maxLength: 32768, nullable: false),
                    comment = table.Column<string>(type: "character varying(32768)", maxLength: 32768, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_indicators", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "periods",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    range_from = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    range_to = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_periods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "positions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_positions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "period_parts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    range_from = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    range_to = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    period_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_period_parts", x => x.id);
                    table.ForeignKey(
                        name: "fk_period_parts_periods_period_id",
                        column: x => x.period_id,
                        principalTable: "periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    name_first_name = table.Column<string>(type: "text", nullable: false),
                    name_last_name = table.Column<string>(type: "text", nullable: false),
                    name_middle_name = table.Column<string>(type: "text", nullable: true),
                    avatar_id = table.Column<Guid>(type: "uuid", nullable: true),
                    position_id = table.Column<Guid>(type: "uuid", nullable: false),
                    registered_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_blocked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees", x => x.id);
                    table.ForeignKey(
                        name: "fk_employees_files_avatar_id",
                        column: x => x.avatar_id,
                        principalTable: "files",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_employees_positions_position_id",
                        column: x => x.position_id,
                        principalTable: "positions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "specialties",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    position_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_specialties", x => x.id);
                    table.ForeignKey(
                        name: "fk_specialties_positions_position_id",
                        column: x => x.position_id,
                        principalTable: "positions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "character varying(32768)", maxLength: 32768, nullable: false),
                    comment_block_id = table.Column<Guid>(type: "uuid", nullable: false),
                    written_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comments", x => x.id);
                    table.ForeignKey(
                        name: "fk_comments_comment_blocks_comment_block_id",
                        column: x => x.comment_block_id,
                        principalTable: "comment_blocks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_comments_employees_author_id",
                        column: x => x.author_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    content = table.Column<string>(type: "character varying(32768)", maxLength: 32768, nullable: false),
                    comment_block_id = table.Column<Guid>(type: "uuid", nullable: false),
                    written_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_posts", x => x.id);
                    table.ForeignKey(
                        name: "fk_posts_comment_blocks_comment_block_id",
                        column: x => x.comment_block_id,
                        principalTable: "comment_blocks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_posts_employees_author_id",
                        column: x => x.author_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "requirements",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    note = table.Column<string>(type: "character varying(8192)", maxLength: 8192, nullable: true),
                    weight = table.Column<double>(type: "double precision", nullable: false),
                    period_part_id = table.Column<Guid>(type: "uuid", nullable: false),
                    specialty_id = table.Column<Guid>(type: "uuid", nullable: false),
                    indicator_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requirements", x => x.id);
                    table.ForeignKey(
                        name: "fk_requirements_indicators_indicator_id",
                        column: x => x.indicator_id,
                        principalTable: "indicators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_requirements_period_parts_period_part_id",
                        column: x => x.period_part_id,
                        principalTable: "period_parts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_requirements_specialties_specialty_id",
                        column: x => x.specialty_id,
                        principalTable: "specialties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "specialty_choices",
                columns: table => new
                {
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    specialty_id = table.Column<Guid>(type: "uuid", nullable: false),
                    can_be_changed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_specialty_choices", x => new { x.employee_id, x.period_id });
                    table.ForeignKey(
                        name: "fk_specialty_choices_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_specialty_choices_periods_period_id",
                        column: x => x.period_id,
                        principalTable: "periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_specialty_choices_specialties_specialty_id",
                        column: x => x.specialty_id,
                        principalTable: "specialties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grades",
                columns: table => new
                {
                    requirement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<double>(type: "double precision", nullable: false),
                    graded_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grades", x => new { x.requirement_id, x.employee_id });
                    table.ForeignKey(
                        name: "fk_grades_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_grades_requirements_requirement_id",
                        column: x => x.requirement_id,
                        principalTable: "requirements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "submissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    requirement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    submitted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    comment_block_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_submissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_submissions_comment_blocks_comment_block_id",
                        column: x => x.comment_block_id,
                        principalTable: "comment_blocks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_submissions_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_submissions_requirements_requirement_id",
                        column: x => x.requirement_id,
                        principalTable: "requirements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "submission_files",
                columns: table => new
                {
                    file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    submission_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_submission_files", x => new { x.submission_id, x.file_id });
                    table.ForeignKey(
                        name: "fk_submission_files_files_file_id",
                        column: x => x.file_id,
                        principalTable: "files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_submission_files_submissions_submission_id",
                        column: x => x.submission_id,
                        principalTable: "submissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_comments_author_id",
                table: "comments",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments_comment_block_id",
                table: "comments",
                column: "comment_block_id");

            migrationBuilder.CreateIndex(
                name: "ix_employees_avatar_id",
                table: "employees",
                column: "avatar_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_employees_email",
                table: "employees",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_employees_position_id",
                table: "employees",
                column: "position_id");

            migrationBuilder.CreateIndex(
                name: "ix_grades_employee_id",
                table: "grades",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_indicators_name",
                table: "indicators",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_period_parts_period_id",
                table: "period_parts",
                column: "period_id");

            migrationBuilder.CreateIndex(
                name: "ix_periods_name",
                table: "periods",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_positions_name",
                table: "positions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_posts_author_id",
                table: "posts",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_posts_comment_block_id",
                table: "posts",
                column: "comment_block_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirements_indicator_id",
                table: "requirements",
                column: "indicator_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirements_period_part_id",
                table: "requirements",
                column: "period_part_id");

            migrationBuilder.CreateIndex(
                name: "ix_requirements_specialty_id",
                table: "requirements",
                column: "specialty_id");

            migrationBuilder.CreateIndex(
                name: "ix_specialties_name",
                table: "specialties",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_specialties_position_id",
                table: "specialties",
                column: "position_id");

            migrationBuilder.CreateIndex(
                name: "ix_specialty_choices_period_id",
                table: "specialty_choices",
                column: "period_id");

            migrationBuilder.CreateIndex(
                name: "ix_specialty_choices_specialty_id",
                table: "specialty_choices",
                column: "specialty_id");

            migrationBuilder.CreateIndex(
                name: "ix_submission_files_file_id",
                table: "submission_files",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "ix_submissions_comment_block_id",
                table: "submissions",
                column: "comment_block_id");

            migrationBuilder.CreateIndex(
                name: "ix_submissions_employee_id",
                table: "submissions",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_submissions_requirement_id",
                table: "submissions",
                column: "requirement_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "grades");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "specialty_choices");

            migrationBuilder.DropTable(
                name: "submission_files");

            migrationBuilder.DropTable(
                name: "submissions");

            migrationBuilder.DropTable(
                name: "comment_blocks");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "requirements");

            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "indicators");

            migrationBuilder.DropTable(
                name: "period_parts");

            migrationBuilder.DropTable(
                name: "specialties");

            migrationBuilder.DropTable(
                name: "periods");

            migrationBuilder.DropTable(
                name: "positions");
        }
    }
}
