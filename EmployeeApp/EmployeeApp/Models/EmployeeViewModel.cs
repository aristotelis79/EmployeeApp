using System;
using System.Collections.Generic;

namespace EmployeeApp.Models
{
    public class EmployeeViewModel
    {
        public Guid EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime EmpDateOfHire { get; set; }

        public Guid? EmpSupervisorId { get; set; }
        public EmployeeViewModel EmpSupervisor { get; set; }

        public IList<AttributeViewModel> EmployeeAttributes { get; set; } = new List<AttributeViewModel>();
    }
}