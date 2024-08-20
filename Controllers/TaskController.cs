using Microsoft.AspNetCore.Mvc;
using TM2.Data;
using TM2.Models;

namespace TM2.Controllers
{
    public class TaskController : Controller
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateTask(int id, string name, string description)
        {
            var task = new Tasks
            {
                Id = id,
                Name = name,
                Description = description,
                DueTime = DateTime.Now
            };

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(Tasks task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return RedirectToAction("Task","Home");
            }
            return View(task);
        }

        [HttpGet]
        public IActionResult EditTask(int id)
            {
           
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound(); 
            }

            return View(task); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTask(int id, Tasks task)
        {
            var existingTask = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Name = task.Name;
            existingTask.Description = task.Description;
            existingTask.DueTime = task.DueTime;

            _context.Update(existingTask);
            _context.SaveChanges();

            return RedirectToAction("Task", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home"); 
        }
    }
}
