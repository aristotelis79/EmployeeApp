using System;
using System.Collections.Generic;

namespace EmployeeApp.Data.Entities
{
    public partial class Attribute : BaseEntity
    {
        public Attribute()
        {
            EmployeeAttribute = new HashSet<EmployeeAttribute>();
        }

        public Guid AttrId { get; set; }
        public string AttrName { get; set; }
        public string AttrValue { get; set; }

        public virtual ICollection<EmployeeAttribute> EmployeeAttribute { get; set; }
    }
}