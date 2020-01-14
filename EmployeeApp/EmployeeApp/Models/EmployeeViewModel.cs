using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        
        [Required]
        [DisplayName("Employee Name")]
        public string EmpName { get; set; }

        [DisplayName("Date of Hire")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime EmpDateOfHire { get; set; } = DateTime.UtcNow;

        [DisplayName("Supervisor")]
        public Guid? EmpSupervisorId { get; set; }

        [DisplayName("Supervisor Name")]
        public string EmpSupervisorName { get; set; }

        public IList<AttributeViewModel> EmployeeAttributes { get; set; } = new List<AttributeViewModel>();
    }
}