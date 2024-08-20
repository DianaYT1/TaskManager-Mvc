using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TM2.Models;

namespace TM2.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Tasks> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.Creator)
                .WithMany() 
                .HasForeignKey(t => t.CreatorId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Tasks>()
                .HasMany(t => t.Collaborators)
                .WithMany(u => u.CollaboratedTasks)
                .UsingEntity(j => j.ToTable("TaskCollaborators"));
        }
    }
}
