﻿// <auto-generated />
using System;
using KpiV3.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KpiV3.Domain.Migrations
{
    [DbContext(typeof(KpiContext))]
    [Migration("20220612232202_FixIndexes")]
    partial class FixIndexes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("KpiV3.Domain.Comments.DataContracts.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<Guid>("CommentBlockId")
                        .HasColumnType("uuid")
                        .HasColumnName("comment_block_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(32768)
                        .HasColumnType("character varying(32768)")
                        .HasColumnName("content");

                    b.Property<DateTimeOffset>("WrittenDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("written_date");

                    b.HasKey("Id")
                        .HasName("pk_comments");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_comments_author_id");

                    b.HasIndex("CommentBlockId")
                        .HasDatabaseName("ix_comments_comment_block_id");

                    b.ToTable("comments", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.Comments.DataContracts.CommentBlock", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_comment_blocks");

                    b.ToTable("comment_blocks", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.Employees.DataContracts.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("AvatarId")
                        .HasColumnType("uuid")
                        .HasColumnName("avatar_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("email");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("boolean")
                        .HasColumnName("is_blocked");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasColumnName("password_hash");

                    b.Property<Guid>("PositionId")
                        .HasColumnType("uuid")
                        .HasColumnName("position_id");

                    b.Property<DateTimeOffset>("RegisteredDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("registered_date");

                    b.HasKey("Id")
                        .HasName("pk_employees");

                    b.HasIndex("AvatarId")
                        .IsUnique()
                        .HasDatabaseName("ix_employees_avatar_id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_employees_email");

                    b.HasIndex("PositionId")
                        .HasDatabaseName("ix_employees_position_id");

                    b.ToTable("employees", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.Files.DataContract.FileMetadata", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("content_type");

                    b.Property<long>("Length")
                        .HasColumnType("bigint")
                        .HasColumnName("length");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.HasKey("Id")
                        .HasName("pk_files");

                    b.HasIndex("OwnerId")
                        .HasDatabaseName("ix_files_owner_id");

                    b.ToTable("files", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.Grades.DataContracts.Grade", b =>
                {
                    b.Property<Guid>("RequirementId")
                        .HasColumnType("uuid")
                        .HasColumnName("requirement_id");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid")
                        .HasColumnName("employee_id");

                    b.Property<DateTimeOffset>("GradedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("graded_date");

                    b.Property<double>("Value")
                        .HasColumnType("double precision")
                        .HasColumnName("value");

                    b.HasKey("RequirementId", "EmployeeId")
                        .HasName("pk_grades");

                    b.HasIndex("EmployeeId")
                        .HasDatabaseName("ix_grades_employee_id");

                    b.ToTable("grades", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.Indicators.DataContracts.Indicator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Comment")
                        .HasMaxLength(32768)
                        .HasColumnType("character varying(32768)")
                        .HasColumnName("comment");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(32768)
                        .HasColumnType("character varying(32768)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_indicators");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_indicators_name");

                    b.ToTable("indicators", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.PeriodParts.DataContracts.PeriodPart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<Guid>("PeriodId")
                        .HasColumnType("uuid")
                        .HasColumnName("period_id");

                    b.HasKey("Id")
                        .HasName("pk_period_parts");

                    b.HasIndex("PeriodId")
                        .HasDatabaseName("ix_period_parts_period_id");

                    b.ToTable("period_parts", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.Periods.DataContracts.Period", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_periods");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_periods_name");

                    b.ToTable("periods", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.Positions.DataContracts.Position", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("name");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_positions");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_positions_name");

                    b.ToTable("positions", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.Posts.DataContracts.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<Guid>("CommentBlockId")
                        .HasColumnType("uuid")
                        .HasColumnName("comment_block_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(32768)
                        .HasColumnType("character varying(32768)")
                        .HasColumnName("content");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasColumnName("title");

                    b.Property<DateTimeOffset>("WrittenDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("written_date");

                    b.HasKey("Id")
                        .HasName("pk_posts");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_posts_author_id");

                    b.HasIndex("CommentBlockId")
                        .HasDatabaseName("ix_posts_comment_block_id");

                    b.ToTable("posts", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.Requirements.DataContracts.Requirement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("IndicatorId")
                        .HasColumnType("uuid")
                        .HasColumnName("indicator_id");

                    b.Property<string>("Note")
                        .HasMaxLength(8192)
                        .HasColumnType("character varying(8192)")
                        .HasColumnName("note");

                    b.Property<Guid>("PeriodPartId")
                        .HasColumnType("uuid")
                        .HasColumnName("period_part_id");

                    b.Property<Guid>("SpecialtyId")
                        .HasColumnType("uuid")
                        .HasColumnName("specialty_id");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision")
                        .HasColumnName("weight");

                    b.HasKey("Id")
                        .HasName("pk_requirements");

                    b.HasIndex("IndicatorId")
                        .HasDatabaseName("ix_requirements_indicator_id");

                    b.HasIndex("PeriodPartId")
                        .HasDatabaseName("ix_requirements_period_part_id");

                    b.HasIndex("SpecialtyId")
                        .HasDatabaseName("ix_requirements_specialty_id");

                    b.ToTable("requirements", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.Specialties.DataContracts.Specialty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("name");

                    b.Property<Guid>("PositionId")
                        .HasColumnType("uuid")
                        .HasColumnName("position_id");

                    b.HasKey("Id")
                        .HasName("pk_specialties");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_specialties_name");

                    b.HasIndex("PositionId")
                        .HasDatabaseName("ix_specialties_position_id");

                    b.ToTable("specialties", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.SpecialtyChoices.DataContracts.SpecialtyChoice", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid")
                        .HasColumnName("employee_id");

                    b.Property<Guid>("PeriodId")
                        .HasColumnType("uuid")
                        .HasColumnName("period_id");

                    b.Property<bool>("CanBeChanged")
                        .HasColumnType("boolean")
                        .HasColumnName("can_be_changed");

                    b.Property<Guid>("SpecialtyId")
                        .HasColumnType("uuid")
                        .HasColumnName("specialty_id");

                    b.HasKey("EmployeeId", "PeriodId")
                        .HasName("pk_specialty_choices");

                    b.HasIndex("PeriodId")
                        .HasDatabaseName("ix_specialty_choices_period_id");

                    b.HasIndex("SpecialtyId")
                        .HasDatabaseName("ix_specialty_choices_specialty_id");

                    b.ToTable("specialty_choices", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.Submissions.DataContracts.Submission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CommentBlockId")
                        .HasColumnType("uuid")
                        .HasColumnName("comment_block_id");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid")
                        .HasColumnName("employee_id");

                    b.Property<Guid>("RequirementId")
                        .HasColumnType("uuid")
                        .HasColumnName("requirement_id");

                    b.Property<DateTimeOffset>("SubmittedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("submitted_date");

                    b.HasKey("Id")
                        .HasName("pk_submissions");

                    b.HasIndex("CommentBlockId")
                        .HasDatabaseName("ix_submissions_comment_block_id");

                    b.HasIndex("EmployeeId")
                        .HasDatabaseName("ix_submissions_employee_id");

                    b.HasIndex("RequirementId")
                        .HasDatabaseName("ix_submissions_requirement_id");

                    b.ToTable("submissions", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.Submissions.DataContracts.SubmissionFile", b =>
                {
                    b.Property<Guid>("SubmissionId")
                        .HasColumnType("uuid")
                        .HasColumnName("submission_id");

                    b.Property<Guid>("FileId")
                        .HasColumnType("uuid")
                        .HasColumnName("file_id");

                    b.HasKey("SubmissionId", "FileId")
                        .HasName("pk_submission_files");

                    b.HasIndex("FileId")
                        .HasDatabaseName("ix_submission_files_file_id");

                    b.ToTable("submission_files", (string)null);
                });

            modelBuilder.Entity("KpiV3.Domain.Comments.DataContracts.Comment", b =>
                {
                    b.HasOne("KpiV3.Domain.Employees.DataContracts.Employee", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_comments_employees_author_id");

                    b.HasOne("KpiV3.Domain.Comments.DataContracts.CommentBlock", "CommentBlock")
                        .WithMany()
                        .HasForeignKey("CommentBlockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_comments_comment_blocks_comment_block_id");

                    b.Navigation("Author");

                    b.Navigation("CommentBlock");
                });

            modelBuilder.Entity("KpiV3.Domain.Employees.DataContracts.Employee", b =>
                {
                    b.HasOne("KpiV3.Domain.Files.DataContract.FileMetadata", "Avatar")
                        .WithOne()
                        .HasForeignKey("KpiV3.Domain.Employees.DataContracts.Employee", "AvatarId")
                        .HasConstraintName("fk_employees_files_avatar_id");

                    b.HasOne("KpiV3.Domain.Positions.DataContracts.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_employees_positions_position_id");

                    b.OwnsOne("KpiV3.Domain.Common.DataContracts.Name", "Name", b1 =>
                        {
                            b1.Property<Guid>("EmployeeId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("name_first_name");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("name_last_name");

                            b1.Property<string>("MiddleName")
                                .HasColumnType("text")
                                .HasColumnName("name_middle_name");

                            b1.HasKey("EmployeeId");

                            b1.ToTable("employees");

                            b1.WithOwner()
                                .HasForeignKey("EmployeeId")
                                .HasConstraintName("fk_employees_employees_id");
                        });

                    b.Navigation("Avatar");

                    b.Navigation("Name")
                        .IsRequired();

                    b.Navigation("Position");
                });

            modelBuilder.Entity("KpiV3.Domain.Files.DataContract.FileMetadata", b =>
                {
                    b.HasOne("KpiV3.Domain.Employees.DataContracts.Employee", "Owner")
                        .WithMany("UploadedFiles")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_files_employees_employee_id");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("KpiV3.Domain.Grades.DataContracts.Grade", b =>
                {
                    b.HasOne("KpiV3.Domain.Employees.DataContracts.Employee", "Employee")
                        .WithMany("Grades")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_grades_employees_employee_id");

                    b.HasOne("KpiV3.Domain.Requirements.DataContracts.Requirement", "Requirement")
                        .WithMany("Grades")
                        .HasForeignKey("RequirementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_grades_requirements_requirement_id");

                    b.Navigation("Employee");

                    b.Navigation("Requirement");
                });

            modelBuilder.Entity("KpiV3.Domain.PeriodParts.DataContracts.PeriodPart", b =>
                {
                    b.HasOne("KpiV3.Domain.Periods.DataContracts.Period", "Period")
                        .WithMany("PeriodParts")
                        .HasForeignKey("PeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_period_parts_periods_period_id");

                    b.OwnsOne("KpiV3.Domain.Common.DataContracts.DateRange", "Range", b1 =>
                        {
                            b1.Property<Guid>("PeriodPartId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<DateTimeOffset>("From")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("range_from");

                            b1.Property<DateTimeOffset>("To")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("range_to");

                            b1.HasKey("PeriodPartId");

                            b1.ToTable("period_parts");

                            b1.WithOwner()
                                .HasForeignKey("PeriodPartId")
                                .HasConstraintName("fk_period_parts_period_parts_id");
                        });

                    b.Navigation("Period");

                    b.Navigation("Range")
                        .IsRequired();
                });

            modelBuilder.Entity("KpiV3.Domain.Periods.DataContracts.Period", b =>
                {
                    b.OwnsOne("KpiV3.Domain.Common.DataContracts.DateRange", "Range", b1 =>
                        {
                            b1.Property<Guid>("PeriodId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<DateTimeOffset>("From")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("range_from");

                            b1.Property<DateTimeOffset>("To")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("range_to");

                            b1.HasKey("PeriodId");

                            b1.ToTable("periods");

                            b1.WithOwner()
                                .HasForeignKey("PeriodId")
                                .HasConstraintName("fk_periods_periods_id");
                        });

                    b.Navigation("Range")
                        .IsRequired();
                });

            modelBuilder.Entity("KpiV3.Domain.Posts.DataContracts.Post", b =>
                {
                    b.HasOne("KpiV3.Domain.Employees.DataContracts.Employee", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_posts_employees_author_id");

                    b.HasOne("KpiV3.Domain.Comments.DataContracts.CommentBlock", "CommentBlock")
                        .WithMany()
                        .HasForeignKey("CommentBlockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_posts_comment_blocks_comment_block_id");

                    b.Navigation("Author");

                    b.Navigation("CommentBlock");
                });

            modelBuilder.Entity("KpiV3.Domain.Requirements.DataContracts.Requirement", b =>
                {
                    b.HasOne("KpiV3.Domain.Indicators.DataContracts.Indicator", "Indicator")
                        .WithMany()
                        .HasForeignKey("IndicatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_requirements_indicators_indicator_id");

                    b.HasOne("KpiV3.Domain.PeriodParts.DataContracts.PeriodPart", "PeriodPart")
                        .WithMany()
                        .HasForeignKey("PeriodPartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_requirements_period_parts_period_part_id");

                    b.HasOne("KpiV3.Domain.Specialties.DataContracts.Specialty", "Specialty")
                        .WithMany("Requirements")
                        .HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_requirements_specialties_specialty_id");

                    b.Navigation("Indicator");

                    b.Navigation("PeriodPart");

                    b.Navigation("Specialty");
                });

            modelBuilder.Entity("KpiV3.Domain.Specialties.DataContracts.Specialty", b =>
                {
                    b.HasOne("KpiV3.Domain.Positions.DataContracts.Position", "Position")
                        .WithMany("Specialties")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_specialties_positions_position_id");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("KpiV3.Domain.SpecialtyChoices.DataContracts.SpecialtyChoice", b =>
                {
                    b.HasOne("KpiV3.Domain.Employees.DataContracts.Employee", "Employee")
                        .WithMany("SpecialtyChoices")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_specialty_choices_employees_employee_id");

                    b.HasOne("KpiV3.Domain.Periods.DataContracts.Period", "Period")
                        .WithMany()
                        .HasForeignKey("PeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_specialty_choices_periods_period_id");

                    b.HasOne("KpiV3.Domain.Specialties.DataContracts.Specialty", "Specialty")
                        .WithMany()
                        .HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_specialty_choices_specialties_specialty_id");

                    b.Navigation("Employee");

                    b.Navigation("Period");

                    b.Navigation("Specialty");
                });

            modelBuilder.Entity("KpiV3.Domain.Submissions.DataContracts.Submission", b =>
                {
                    b.HasOne("KpiV3.Domain.Comments.DataContracts.CommentBlock", "CommentBlock")
                        .WithMany()
                        .HasForeignKey("CommentBlockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_submissions_comment_blocks_comment_block_id");

                    b.HasOne("KpiV3.Domain.Employees.DataContracts.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_submissions_employees_employee_id");

                    b.HasOne("KpiV3.Domain.Requirements.DataContracts.Requirement", "Requirement")
                        .WithMany("Submissions")
                        .HasForeignKey("RequirementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_submissions_requirements_requirement_id");

                    b.Navigation("CommentBlock");

                    b.Navigation("Employee");

                    b.Navigation("Requirement");
                });

            modelBuilder.Entity("KpiV3.Domain.Submissions.DataContracts.SubmissionFile", b =>
                {
                    b.HasOne("KpiV3.Domain.Files.DataContract.FileMetadata", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_submission_files_files_file_id");

                    b.HasOne("KpiV3.Domain.Submissions.DataContracts.Submission", "Submission")
                        .WithMany("Files")
                        .HasForeignKey("SubmissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_submission_files_submissions_submission_id");

                    b.Navigation("File");

                    b.Navigation("Submission");
                });

            modelBuilder.Entity("KpiV3.Domain.Employees.DataContracts.Employee", b =>
                {
                    b.Navigation("Grades");

                    b.Navigation("SpecialtyChoices");

                    b.Navigation("UploadedFiles");
                });

            modelBuilder.Entity("KpiV3.Domain.Periods.DataContracts.Period", b =>
                {
                    b.Navigation("PeriodParts");
                });

            modelBuilder.Entity("KpiV3.Domain.Positions.DataContracts.Position", b =>
                {
                    b.Navigation("Specialties");
                });

            modelBuilder.Entity("KpiV3.Domain.Requirements.DataContracts.Requirement", b =>
                {
                    b.Navigation("Grades");

                    b.Navigation("Submissions");
                });

            modelBuilder.Entity("KpiV3.Domain.Specialties.DataContracts.Specialty", b =>
                {
                    b.Navigation("Requirements");
                });

            modelBuilder.Entity("KpiV3.Domain.Submissions.DataContracts.Submission", b =>
                {
                    b.Navigation("Files");
                });
#pragma warning restore 612, 618
        }
    }
}
