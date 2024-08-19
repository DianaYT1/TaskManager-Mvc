using Microsoft.AspNetCore.Mvc;
using TM2.Models;

namespace TM2.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateTask(int id)
        {
            var task = new Tasks
            {
                Id = id,
                Name = "Example Task",
                Description = "Task Description",
                DueTime = DateTime.Now
            };

            return View(task);
        }
    }
}
