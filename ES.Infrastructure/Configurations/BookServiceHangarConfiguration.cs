using ES.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ES.Infrastructure.Configurations
{
    public class BookServiceHangarConfiguration : IEntityTypeConfiguration<BookServiceHangar>
    {
        public void Configure(EntityTypeBuilder<BookServiceHangar> builder)
        {
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(h => h.BookServiceRequest)
                   .WithMany(b => b.Hangars)
                   .HasForeignKey(h => h.BookServiceRequestId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
