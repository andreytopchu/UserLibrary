using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal;

public class ApplicantDbContext : DbContext
{
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Applicant> Applicants => Set<Applicant>();

    public ApplicantDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>(roleBuilder =>
        {
            roleBuilder.HasKey(role => role.Id);

            roleBuilder.HasIndex(role => role.Name).IsUnique();
        });

        modelBuilder.Entity<User>(userBuilder =>
        {
            userBuilder.HasKey(u => u.Id);

            userBuilder
                .HasOne(user => user.Role)
                .WithMany()
                .HasForeignKey("RoleId");

            userBuilder
                .Navigation(user => user.Role)
                .AutoInclude();
        });

        modelBuilder.Entity<Applicant>(applicantBuilder =>
        {
            applicantBuilder.HasKey(a => a.Id);

            applicantBuilder
                .HasOne(a => a.Author)
                .WithMany()
                .HasForeignKey("UserId");

            applicantBuilder
                .Navigation(a => a.Author)
                .AutoInclude();

            applicantBuilder
                .OwnsOne(
                    applicant => applicant.Document,
                    applicantDocumentBuilder =>
                    {
                        applicantDocumentBuilder.Property<Guid>("ApplicantId");

                        applicantDocumentBuilder
                            .WithOwner()
                            .HasForeignKey("ApplicantId");

                        applicantDocumentBuilder.ToTable("ApplicantDocument");
                    });

            applicantBuilder
                .OwnsOne(
                    applicant => applicant.Workflow,
                    workflowBuilder =>
                    {
                        workflowBuilder.HasKey(x => x.Id);

                        workflowBuilder.Property<Guid>("ApplicantId");

                        workflowBuilder
                            .WithOwner()
                            .HasForeignKey("ApplicantId");

                        workflowBuilder
                            .OwnsMany(
                                workflow => workflow.Steps,
                                workflowStepBuilder =>
                                {
                                    workflowStepBuilder.Property<Guid>("WorkflowId");

                                    workflowStepBuilder
                                        .WithOwner()
                                        .HasForeignKey("WorkflowId");

                                    workflowStepBuilder.Property<Guid>("UserId");

                                    workflowStepBuilder
                                        .HasOne(step => step.ApprovedUser)
                                        .WithMany()
                                        .HasForeignKey("UserId");

                                    workflowStepBuilder.ToTable("ApplicantWorkflowStep");
                                });

                        workflowBuilder.ToTable("ApplicantWorkflow");
                    });
        });
    }
}