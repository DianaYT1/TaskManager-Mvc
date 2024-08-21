using Microsoft.AspNetCore.Mvc;
using TM2.Data;
using TM2.ViewModels;

namespace TM2.Controllers
{
    public class CalendarController : Controller
    {
        private readonly AppDbContext _context;

        public CalendarController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? month, int? year)
        {
            
            var currentDate = DateTime.Now;

            var displayMonth = month ?? currentDate.Month;
            var displayYear = year ?? currentDate.Year;

            
            var firstDayOfMonth = new DateTime(displayYear, displayMonth, 1);

            
            var daysInMonth = DateTime.DaysInMonth(displayYear, displayMonth);

            
            var tasksForMonth = _context.Tasks
                .Where(t => t.DueTime.Year == displayYear && t.DueTime.Month == displayMonth)
                .ToList();

            var viewModel = new CalendarViewModel
            {
                DisplayMonth = displayMonth,
                DisplayYear = displayYear,
                FirstDayOfMonth = firstDayOfMonth,
                DaysInMonth = daysInMonth,
                Tasks = tasksForMonth
            };

            return View(viewModel);
        }
    }

}
