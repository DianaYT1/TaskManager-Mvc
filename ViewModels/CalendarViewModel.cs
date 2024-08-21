using TM2.Models;

namespace TM2.ViewModels
{
    public class CalendarViewModel
    {
        public int DisplayMonth { get; set; }
        public int DisplayYear { get; set; }
        public DateTime FirstDayOfMonth { get; set; }
        public int DaysInMonth { get; set; }
        public List<Tasks> Tasks { get; set; }
    }
}
