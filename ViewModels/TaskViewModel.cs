using System.ComponentModel.DataAnnotations;

namespace TM2.ViewModels
{
    public class TaskViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueTime { get; set; }
    }
}
