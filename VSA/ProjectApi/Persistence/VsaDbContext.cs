using Microsoft.EntityFrameworkCore;
using ProjectApi.Domain.Persons;
using ProjectApi.Domain.Projects;

namespace ProjectApi.Persistence;

public class VsaDbContext : DbContext
{
    public VsaDbContext(DbContextOptions<VsaDbContext> options) : base(options) { }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Person> People => Set<Person>();
    public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();
    public DbSet<ProjectAssignment> ProjectAssignments => Set<ProjectAssignment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Tüm IEntityTypeConfiguration<T> implementasyonlarını otomatik uygular
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VsaDbContext).Assembly);
    }
}
