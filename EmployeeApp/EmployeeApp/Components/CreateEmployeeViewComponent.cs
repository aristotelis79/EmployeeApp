﻿using EmployeeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Components
{

    [ViewComponent(Name = "CreateEmployee")]
    public class CreateEmployeeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(EmployeeViewModel model)
        {
            return View(model ?? EmployeeViewModel.New());
        }
    }
}