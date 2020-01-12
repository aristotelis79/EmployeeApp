using EmployeeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Components
{

    [ViewComponent(Name = "Employee")]
    public class EmployeeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(EmployeeViewModel model)
        {
            return View(model ?? EmployeeViewModel.New());
        }
    }
}