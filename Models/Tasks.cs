using System.ComponentModel.DataAnnotations.Schema;

namespace TM2.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueTime { get; set; }
        public string CreatorId { get; set; }
        public virtual Users Creator { get; set; }

        public virtual ICollection<Users> Collaborators { get; set; }
    }
}