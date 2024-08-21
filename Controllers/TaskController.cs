using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

        public async Task<IActionResult> CreateTask(int id, string name, string description, string collaboratorEmails)
        {
            var task = new Tasks
            {
                Id = id,
                Name = name,
                Description = description,
                DueTime = DateTime.Now
            };

            var emailList = collaboratorEmails?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(e => e.Trim()).ToList();

            if (emailList != null && emailList.Count > 0)
            {
                task.Collaborators = new List<Users>();

                foreach (var email in emailList)
                {
                    var user = _context.Users.FirstOrDefault(u => u.Email == email);
                    if (user != null)
                    {
                        task.Collaborators.Add(user);
                    }
                    else
                    {
                        ModelState.AddModelError("", $"User with email {email} not found.");
                    }
                }
            }

            return View(task);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(Tasks task, string collaboratorEmails)
        {
             
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

                if (currentUserId == null)
                {
                    ModelState.AddModelError("", "Unable to determine the current user.");
                    return View(task);
                }

                task.CreatorId = currentUserId;

                var emailList = collaboratorEmails?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(e => e.Trim()).ToList();

                if (emailList != null && emailList.Count > 0)
                {
                    task.Collaborators = new List<Users>();

                    foreach (var email in emailList)
                    {
                        var user = _context.Users.FirstOrDefault(u => u.Email == email);
                        if (user != null)
                        {
                            task.Collaborators.Add(user);
                        }
                        else
                        {
                            ModelState.AddModelError("", $"User with email {email} not found.");
                        }
                    }
                }

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return RedirectToAction("Task", "Home");
        }


        [HttpGet]
        public IActionResult EditTask(int id)
        {
            var task = _context.Tasks
                               .Include(t => t.Collaborators) 
                               .FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(int id, Tasks task, string collaboratorEmails)
        {
            var existingTask = _context.Tasks.Include(t => t.Collaborators)
                                             .FirstOrDefault(t => t.Id == id);

            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Name = task.Name;
            existingTask.Description = task.Description;
            existingTask.DueTime = task.DueTime;

            var emailList = collaboratorEmails?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(e => e.Trim()).ToList();

            if (emailList != null && emailList.Count > 0)
            {
                existingTask.Collaborators.Clear(); 

                foreach (var email in emailList)
                {
                    var user = _context.Users.FirstOrDefault(u => u.Email == email);
                    if (user != null)
                    {
                        existingTask.Collaborators.Add(user);
                    }
                    else
                    {
                        ModelState.AddModelError("", $"User with email {email} not found.");
                    }
                }
            }

            _context.Update(existingTask);
            await _context.SaveChangesAsync();
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

            return RedirectToAction("Task", "Home"); 
        }
    }
}
