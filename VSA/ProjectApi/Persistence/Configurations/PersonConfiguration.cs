using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectApi.Domain.Persons;

namespace ProjectApi.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.FullName)
            .IsRequired()
            .HasMaxLength(300);

        builder.OwnsOne(p => p.Email, e =>
        {
            e.Property(em => em.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(320);

            e.HasIndex(em => em.Value).IsUnique();
        });

        builder.Ignore(p => p.DomainEvents);

        builder.ToTable("Persons");
    }
}
