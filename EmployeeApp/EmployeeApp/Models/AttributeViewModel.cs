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
        [DisplayName("Name")]
        public string AttrName { get; set; }

        [Required]
        [DisplayName("Value")]
        public string AttrValue { get; set; }
    }
}