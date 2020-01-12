using System;
using System.Collections.Generic;
using EmployeeApp.Data.Entities;
using Attribute = EmployeeApp.Data.Entities.Attribute;

namespace EmployeeApp.Models.Mappers
{
    public static class EmployeeMapper
    {
        public static EmployeeViewModel ToViewModel(this Employee entity)
        {
            var model = new EmployeeViewModel
            {
                EmpId = entity.EmpId,
                EmpName = entity.EmpName,
                EmpDateOfHire = entity.EmpDateOfHire,
                EmpSupervisorId = entity.EmpSupervisor,
                EmpSupervisorName = entity.EmpSupervisorNavigation?.EmpName
            };

            foreach (var p in entity.EmployeeAttribute)
            {
                model.EmployeeAttributes.Add(new AttributeViewModel
                {
                    AttrId = p.EmpAttrAttribute.AttrId,
                    AttrName = p.EmpAttrAttribute.AttrName,
                    AttrValue = p.EmpAttrAttribute.AttrValue
                });
            }

            return model;
        }

        public static List<EmployeeViewModel> ToViewModel(this List<Employee> contacts)
        {
            var model = new List<EmployeeViewModel>();

            contacts.ForEach(c => model.Add(c.ToViewModel()));

            return model;
        }

        public static Employee ToEntity(this EmployeeViewModel model)
        {
            var empId = model.EmpId != null && model.EmpId != Guid.Empty 
                                                                        ? model.EmpId 
                                                                        : Guid.NewGuid();

            var entity = new Employee
            {
                EmpId = empId,
                EmpName = model.EmpName,
                EmpDateOfHire = model.EmpDateOfHire,
                EmpSupervisor = model.EmpSupervisorId
            };
            foreach (var p in model.EmployeeAttributes)
            {
                entity.EmployeeAttribute.Add(new EmployeeAttribute
                {
                    EmpAttrEmployeeId = empId,
                    EmpAttrAttributeId = p.AttrId,
                    EmpAttrAttribute = new Attribute
                    {
                        AttrId = p.AttrId != null && p.AttrId != Guid.Empty 
                                                                            ? p.AttrId
                                                                            : Guid.NewGuid(),
                        AttrName = p.AttrName,
                        AttrValue = p.AttrValue
                    }
                });
            }
            return entity;
        }
    }
}