using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserSystemService.Models;

namespace UserSystemService.Configurations;

public class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
{
    private const int TaskNameLength = 255;
    private const int TaskDescriptionLength = 350;
    
    public void Configure(EntityTypeBuilder<UserTask> builder)
    {
        builder.ToTable("UserTasks")
            .HasKey(e => e.ID);

        builder.Property(e => e.UserID);
        
        builder.Property(e => e.TaskName)
            .IsRequired()
            .HasMaxLength(TaskNameLength);

        builder.Property(e => e.TaskDescription)
            .HasMaxLength(TaskDescriptionLength);

        builder.Property(e => e.TaskStatus)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.ModifiedAt)
            .IsRequired();

        builder.Property(e => e.IsDeleted)
            .IsRequired();

        builder.Property(e => e.RowVersion)
            .IsConcurrencyToken();
    }
}