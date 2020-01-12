using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EmployeeApp.Models
{
    [Serializable]
    public class EmployeeViewModel
    {
        public static EmployeeViewModel New()
        {
            var model = new EmployeeViewModel();
            model.EmployeeAttributes.Add(new AttributeViewModel());
            return model;
        }

        public Guid EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime EmpDateOfHire { get; set; }

        [DisplayName("Supervisor")]
        public Guid? EmpSupervisorId { get; set; }
        public string EmpSupervisorName { get; set; }

        public IList<AttributeViewModel> EmployeeAttributes { get; set; } = new List<AttributeViewModel>();
    }
}