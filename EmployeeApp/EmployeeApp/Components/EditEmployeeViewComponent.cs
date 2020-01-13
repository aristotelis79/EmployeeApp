using EmployeeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Components
{

    [ViewComponent(Name = "EditEmployee")]
    public class EditEmployeeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(EmployeeViewModel model)
        {
            return View(model ?? EmployeeViewModel.New());
        }
    }
}