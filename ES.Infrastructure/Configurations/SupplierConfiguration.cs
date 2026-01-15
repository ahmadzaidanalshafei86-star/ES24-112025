using ES.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ES.Infrastructure.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");

            builder.HasKey(s => s.Id);

            // Required fields
            builder.Property(s => s.UserType)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(s => s.CompanyName)
                   .HasMaxLength(250);

            builder.Property(s => s.CompanySector)
                   .HasMaxLength(50);

            builder.Property(s => s.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(s => s.EmailAddress)
                   .HasMaxLength(250);

            // Optional fields
            builder.Property(s => s.RegistrationNumber)
                   .HasMaxLength(100);

            builder.Property(s => s.FaxNumber)
                   .HasMaxLength(100);

            builder.Property(s => s.WebsiteUrl)
                   .HasMaxLength(500);

            builder.Property(s => s.Address)
                   .HasMaxLength(1000);

            // File path columns
            builder.Property(s => s.CommercialRegisterPath)
                   .HasMaxLength(500);

            builder.Property(s => s.ProfessionalLicensePath)
                   .HasMaxLength(500);

            builder.Property(s => s.ListOfKeyAchievementsPath)
                   .HasMaxLength(500);

            builder.Property(s => s.ListOfMajorClientsPath)
                   .HasMaxLength(500);

            builder.Property(s => s.CopyOfRegistrationCertificatePath)
                   .HasMaxLength(500);

            builder.Property(s => s.CreatedAt)
                   .IsRequired();

            // Relation with SupplierMaterials
            builder.HasMany(s => s.SupplierMaterials)
                   .WithOne(sm => sm.Supplier)
                   .HasForeignKey(sm => sm.SupplierId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
