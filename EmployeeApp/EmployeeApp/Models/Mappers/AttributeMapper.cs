using System.Collections.Generic;
using EmployeeApp.Data.Entities;
using Attribute = System.Attribute;

namespace EmployeeApp.Models.Mappers
{
    public static class AttributeMapper
    {
        public static AttributeViewModel ToViewModel(this EmployeeAttribute entity)
        {
            var model = new AttributeViewModel
            {
                //EmpId = entity.EmpId,
                //EmpName = entity.EmpName,
                //EmpDateOfHire = entity.EmpDateOfHire,
                //EmpSupervisorId = entity.EmpSupervisor,
                //EmpSupervisor = entity.EmpSupervisor != null ? entity.EmpSupervisorNavigation.ToViewModel(): null,
                //EmployeeAttributes = entity.EmployeeAttribute.MapTo()
            };

            return model;
        }

        public static List<AttributeViewModel> ToViewModel(this List<EmployeeAttribute> contacts)
        {
            var model = new List<AttributeViewModel>();

            contacts.ForEach(c => model.Add(c.ToViewModel()));

            return model;
        }
    }
}