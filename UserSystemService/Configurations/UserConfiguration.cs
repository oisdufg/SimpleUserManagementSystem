using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserSystemService.Models;

namespace UserSystemService.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    private const int FirstNameMaxLength = 30;
    private const int LastNameMaxLength = 30;
    private const int MiddleNameMaxLength = 30;
    private const int EmailMaxLength = 255;
    
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users")
            .HasKey(e => e.ID);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(FirstNameMaxLength);
        
        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(LastNameMaxLength);
        
        builder.Property(e => e.MiddleName)
            .HasMaxLength(MiddleNameMaxLength);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(EmailMaxLength);

        builder.Property(e => e.Birthday)
            .IsRequired();

        builder.HasMany(e => e.Tasks)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserID)
            .OnDelete(DeleteBehavior.SetNull);
    }
}