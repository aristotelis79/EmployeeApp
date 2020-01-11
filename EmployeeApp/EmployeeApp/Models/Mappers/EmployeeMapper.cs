using System.Collections.Generic;
using EmployeeApp.Data.Entities;

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
                EmpSupervisor = entity.EmpSupervisor != null ? entity.EmpSupervisorNavigation.ToViewModel(): null,
            };

            //foreach (var p in entity.EmployeeAttribute)
            //{
            //    model.EmployeeAttributes.Add(new AttributeViewModel
            //    {
            //        PhoneType = p.PhoneType,
            //        PhoneNumber = p.PhoneNumber
            //    });
            //}

            return model;
        }

        public static List<EmployeeViewModel> ToViewModel(this List<Employee> contacts)
        {
            var model = new List<EmployeeViewModel>();

            contacts.ForEach(c => model.Add(c.ToViewModel()));

            return model;
        }

        //public static Employee MapTo(this EmployeeViewModel model)
        //{
        //    var entity = new Employee
        //    {
        //        FullName = model.FullName,
        //        Email = model.Email,
        //        Address = model.Address
        //    };
        //    foreach (var p in model.Phones)
        //    {
        //        entity.Phones.Add(new Phone
        //        {
        //            PhoneType = p.PhoneType,
        //            PhoneNumber = p.PhoneNumber
        //        });
        //    }
        //    return entity;
        //} 
    }
}