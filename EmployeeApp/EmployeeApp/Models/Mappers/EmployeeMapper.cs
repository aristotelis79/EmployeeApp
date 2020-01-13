using System;
using System.Collections.Generic;
using System.Linq;
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
                EmpSupervisor = model.EmpSupervisorId == null || model.EmpSupervisorId == Guid.Empty ? null : model.EmpSupervisorId
            };
            foreach (var p in model.EmployeeAttributes)
            {
                var attrId = p.AttrId != null && p.AttrId != Guid.Empty
                                                                    ? p.AttrId
                                                                    : Guid.NewGuid();
                entity.EmployeeAttribute.Add(new EmployeeAttribute
                {
                    EmpAttrEmployeeId = empId,
                    EmpAttrAttributeId = attrId,
                    EmpAttrAttribute = new Attribute
                    {
                        AttrId = attrId,
                        AttrName = p.AttrName,
                        AttrValue = p.AttrValue
                    }
                });
            }
            return entity;
        }

        public static Employee UpdateEmployeeEntity(this Employee entity, EmployeeViewModel model)
        {
            entity.EmpDateOfHire = model.EmpDateOfHire;
            entity.EmpName = model.EmpName;
            entity.EmpSupervisor = model.EmpSupervisorId;

            var entityAttributeIds = entity.EmployeeAttribute.Select(s => s.EmpAttrAttribute.AttrId).ToList();
            var modelAttributeIds = model.EmployeeAttributes.Select(s => s.AttrId).ToList();


            //updated Attributes
            var updatedAttributesIds = entityAttributeIds.Intersect(modelAttributeIds);
            foreach (var employeeAttribute in entity.EmployeeAttribute.Where(x => updatedAttributesIds.Contains(x.EmpAttrAttributeId)))
            {
                employeeAttribute.EmpAttrAttribute.AttrName = model.EmployeeAttributes.First(f => f.AttrId == employeeAttribute.EmpAttrAttribute.AttrId).AttrName;
                employeeAttribute.EmpAttrAttribute.AttrValue = model.EmployeeAttributes.First(f => f.AttrId == employeeAttribute.EmpAttrAttribute.AttrId).AttrValue;
            }

            //deleted Attributes
            var deleteAttributesIds = entityAttributeIds.Where(e => modelAttributeIds.All(m => m != e));
            foreach (var employeeAttribute in entity.EmployeeAttribute.Where(x => deleteAttributesIds.Contains(x.EmpAttrAttributeId)))
            {
                entity.EmployeeAttribute.Remove(employeeAttribute);
            }

            //new Attributes
            var newAttributesIds = modelAttributeIds.Where(m => entityAttributeIds.All(e => e != m));

            foreach (var a in model.EmployeeAttributes.Where(x=> newAttributesIds.Contains(x.AttrId)))
            {
                var attrId = a.AttrId != null && a.AttrId != Guid.Empty
                                                                        ? a.AttrId
                                                                        : Guid.NewGuid();
                entity.EmployeeAttribute.Add(new EmployeeAttribute
                {
                    EmpAttrEmployeeId = entity.EmpId,
                    EmpAttrAttributeId = attrId ,
                    EmpAttrAttribute = new Attribute
                    {
                        AttrId = attrId,
                        AttrName = a.AttrName,
                        AttrValue = a.AttrValue
                    }
                });
            }

            return entity;
        }
    }
}