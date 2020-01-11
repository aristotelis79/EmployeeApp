using EmployeeApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeApp.Data.EntityTypeConfigurations
{
    public class AttributeConfiguration : IEntityTypeConfiguration<Attribute>
    {
        public void Configure(EntityTypeBuilder<Attribute> builder)
        {
            builder.ToTable(nameof(Attribute))
                .HasKey(e => e.AttrId);

            builder.Property(e => e.AttrId)
                .HasColumnName("ATTR_ID")
                .ValueGeneratedNever();

            builder.Property(e => e.AttrName)
                .IsRequired()
                .HasColumnName("ATTR_Name")
                .HasMaxLength(50);

            builder.Property(e => e.AttrValue)
                .IsRequired()
                .HasColumnName("ATTR_Value")
                .HasMaxLength(50);
        }
    }
}