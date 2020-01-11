using System;

namespace EmployeeApp.Data.Entities
{
    public partial class EmployeeAttribute : BaseEntity
    {
        public Guid EmpAttrEmployeeId { get; set; }
        public Guid EmpAttrAttributeId { get; set; }

        public virtual Attribute EmpAttrAttribute { get; set; }
        public virtual Employee EmpAttrEmployee { get; set; }
    }
}