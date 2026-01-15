using ES.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ES.Infrastructure.Configurations
{
    public class BookServiceRequestConfiguration : IEntityTypeConfiguration<BookServiceRequest>
    {
        public void Configure(EntityTypeBuilder<BookServiceRequest> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(b => b.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(b => b.EmailAddress)
                   .HasMaxLength(100);

            builder.Property(b => b.Notes)
                   .HasMaxLength(1000);

            builder.Property(b => b.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            // علاقات Hangars
            builder.HasMany(b => b.Hangars)
                   .WithOne(h => h.BookServiceRequest)
                   .HasForeignKey(h => h.BookServiceRequestId)
                   .OnDelete(DeleteBehavior.Cascade);

            // علاقات Refrigators
            builder.HasMany(b => b.Refrigators)
                   .WithOne(r => r.BookServiceRequest)
                   .HasForeignKey(r => r.BookServiceRequestId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
