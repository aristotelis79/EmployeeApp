using EmployeeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Components
{

    [ViewComponent(Name = "DetailsEmployee")]
    public class DetailsEmployeeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(EmployeeViewModel model)
        {
            return View(model ?? EmployeeViewModel.New());
        }
    }
}