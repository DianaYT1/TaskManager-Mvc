using Microsoft.AspNetCore.Identity;

namespace TM2.Models
{
    public class Users :IdentityUser
    {
        public string FullName { get; set; }
        public virtual ICollection<Tasks> CollaboratedTasks { get; set; }
    }
}
