using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeApp.Models
{
    [Serializable]
    public class AttributeViewModel
    {
        public Guid AttrId { get; set; }

        [Required]
        [DisplayName("Attribute Name")]
        public string AttrName { get; set; }

        [Required]
        [DisplayName("Attribute Value")]
        public string AttrValue { get; set; }
    }
}