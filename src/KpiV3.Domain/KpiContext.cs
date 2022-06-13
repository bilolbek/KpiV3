using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Files.DataContract;
using KpiV3.Domain.Grades.DataContracts;
using KpiV3.Domain.Indicators.DataContracts;
using KpiV3.Domain.PeriodParts.DataContracts;
using KpiV3.Domain.Periods.DataContracts;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Posts.DataContracts;
using KpiV3.Domain.Requirements.DataContracts;
using KpiV3.Domain.Specialties.DataContracts;
using KpiV3.Domain.SpecialtyChoices.DataContracts;
using KpiV3.Domain.Submissions.DataContracts;

namespace KpiV3.Domain;

public class KpiContext : DbContext
{
    public KpiContext(DbContextOptions<KpiContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; init; } = default!;
    public DbSet<Position> Positions { get; init; } = default!;
    public DbSet<Specialty> Specialties { get; init; } = default!;
    public DbSet<FileMetadata> Files { get; init; } = default!;
    public DbSet<Period> Periods { get; init; } = default!;
    public DbSet<PeriodPart> PeriodParts { get; init; } = default!;
    public DbSet<SpecialtyChoice> SpecialtyChoices { get; init; } = default!;
    public DbSet<Indicator> Indicators { get; init; } = default!;
    public DbSet<Requirement> Requirements { get; init; } = default!;
    public DbSet<Submission> Submissions { get; init; } = default!;
    public DbSet<SubmissionFile> SubmissionFiles { get; init; } = default!;
    public DbSet<Grade> Grades { get; init; } = default!;
    public DbSet<Post> Posts { get; init; } = default!;
    public DbSet<Comment> Comments { get; init; } = default!;
    public DbSet<CommentBlock> CommentBlocks { get; init; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(t =>
        {
            t.HasIndex(e => e.Email).IsUnique();
            t.Property(e => e.Email).HasMaxLength(128).IsRequired();

            t.Property(e => e.PasswordHash).HasMaxLength(512).IsRequired();

            t
                .HasMany(e => e.UploadedFiles)
                .WithOne(e => e.Owner)
                .HasForeignKey(f => f.OwnerId);

            t
                .HasOne(t => t.Avatar)
                .WithOne()
                .HasForeignKey<Employee>(f => f.AvatarId);

            t.OwnsOne(e => e.Name, name =>
            {
                name.Property(n => n.FirstName).IsRequired();
                name.Property(n => n.LastName).IsRequired();
            });
        });

        modelBuilder.Entity<Position>(t =>
        {
            t.HasIndex(p => p.Name).IsUnique();
            t.Property(p => p.Name).HasMaxLength(128).IsRequired();
        });

        modelBuilder.Entity<Specialty>(t =>
        {
            t.HasIndex(s => s.Name).IsUnique();
            t.Property(s => s.Name).HasMaxLength(128).IsRequired();
        });

        modelBuilder.Entity<FileMetadata>(t =>
        {
            t.Property(f => f.Name).HasMaxLength(256).IsRequired();

            t.Property(f => f.ContentType).HasMaxLength(128).IsRequired();
        });

        modelBuilder.Entity<Period>(t =>
        {
            t.HasIndex(p => p.Name).IsUnique();
            t.Property(p => p.Name).HasMaxLength(256).IsRequired();

            t.OwnsOne(p => p.Range);
        });

        modelBuilder.Entity<PeriodPart>(t =>
        {
            t.Property(p => p.Name).HasMaxLength(256).IsRequired();

            t.OwnsOne(p => p.Range);
        });

        modelBuilder.Entity<SpecialtyChoice>(t =>
        {
            t.HasKey(s => new { s.EmployeeId, s.PeriodId });
        });

        modelBuilder.Entity<Indicator>(t =>
        {
            t.HasIndex(i => i.Name).IsUnique();
            t.Property(i => i.Name).HasMaxLength(256).IsRequired();

            t.Property(i => i.Description).HasMaxLength(1024 * 32).IsRequired();

            t.Property(i => i.Comment).HasMaxLength(1024 * 32);
        });

        modelBuilder.Entity<Requirement>(t =>
        {
            t.Property(r => r.Note).HasMaxLength(1024 * 8);
        });

        modelBuilder.Entity<Submission>(t =>
        {
            t.HasIndex(s => new { s.RequirementId, s.EmployeeId }).IsUnique();
        });

        modelBuilder.Entity<SubmissionFile>(t =>
        {
            t.HasKey(sf => new { sf.SubmissionId, sf.FileId });
        });

        modelBuilder.Entity<Grade>(t =>
        {
            t.HasKey(g => new { g.RequirementId, g.EmployeeId });
        });

        modelBuilder.Entity<Post>(t =>
        {
            t.Property(p => p.Title).HasMaxLength(512).IsRequired();
            t.Property(p => p.Content).HasMaxLength(1024 * 32).IsRequired();
        });

        modelBuilder.Entity<Comment>(t =>
        {
            t.Property(c => c.Content).HasMaxLength(1024 * 32).IsRequired();
        });

        modelBuilder.Entity<CommentBlock>(t =>
        {
        });
    }
}
