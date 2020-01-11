using EmployeeApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeApp.Data.EntityTypeConfigurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable(nameof(Employee))
                .HasKey(e => e.EmpId);

            builder.Property(e => e.EmpId)
                .HasColumnName("EMP_ID")
                .ValueGeneratedNever();

            builder.Property(e => e.EmpDateOfHire)
                .HasColumnName("EMP_DateOfHire")
                .HasColumnType("datetime");

            builder.Property(e => e.EmpName)
                .IsRequired()
                .HasColumnName("EMP_Name")
                .HasMaxLength(100);

            builder.Property(e => e.EmpSupervisor).HasColumnName("EMP_Supervisor");

            builder.HasOne(d => d.EmpSupervisorNavigation)
                .WithMany(p => p.InverseEmpSupervisorNavigation)
                .HasForeignKey(d => d.EmpSupervisor)
                .HasConstraintName("FK_Employee_Employee");
        }
    }
}