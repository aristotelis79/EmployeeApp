using EmployeeApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeApp.Data.EntityTypeConfigurations
{
    public class EmployeeAttributeConfiguration : IEntityTypeConfiguration<EmployeeAttribute>
    {
        public void Configure(EntityTypeBuilder<EmployeeAttribute> builder)
        {
            builder.ToTable(nameof(EmployeeAttribute))
                        .HasKey(e => new { e.EmpAttrEmployeeId, e.EmpAttrAttributeId });

            builder.Property(e => e.EmpAttrEmployeeId)
                    .HasColumnName("EMPATTR_EmployeeID");

            builder.Property(e => e.EmpAttrAttributeId)
                .HasColumnName("EMPATTR_AttributeID");

            builder.HasOne(d => d.EmpAttrAttribute)
                .WithMany(p => p.EmployeeAttribute)
                .HasForeignKey(d => d.EmpAttrAttributeId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK_EmployeeAttribute_Attribute");

            builder.HasOne(d => d.EmpAttrEmployee)
                .WithMany(p => p.EmployeeAttribute)
                .HasForeignKey(d => d.EmpAttrEmployeeId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK_EmployeeAttribute_Employee");
        }
    }
}