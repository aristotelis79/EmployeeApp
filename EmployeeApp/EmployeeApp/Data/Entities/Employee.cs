using System;
using System.Collections.Generic;

namespace EmployeeApp.Data.Entities
{
    public partial class Employee : BaseEntity
    {
        public Employee()
        {
            EmployeeAttribute = new HashSet<EmployeeAttribute>();
            InverseEmpSupervisorNavigation = new HashSet<Employee>();
        }

        public Guid EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime EmpDateOfHire { get; set; }
        public Guid? EmpSupervisor { get; set; }

        public virtual Employee EmpSupervisorNavigation { get; set; }
        public virtual ICollection<EmployeeAttribute> EmployeeAttribute { get; set; }
        public virtual ICollection<Employee> InverseEmpSupervisorNavigation { get; set; }
    }
}