using Microsoft.EntityFrameworkCore;

//ST10445500 - PROG6212 POE - Final Submission

//Models: AppDbContext

//.......................................o0oSTART OF FILEo0o........................................//

namespace CMCS_ST10445500_PROG6212POE_Final.Models
{
    public class AppDbContext : DbContext
    {
        //................................................................//
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AppUser> Users => Set<AppUser>();
        public DbSet<Claim> Claims => Set<Claim>();

        //................................................................//

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //seeding of HR account (cannt be created in app)
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1,
                    Name = "HR Administrator",
                    Email = "hr@cmcs.ac.za",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Hr@2025"),
                    Role = UserRole.HR
                }
            );
        }*/


        //avoid migration issues with BCrypt by removing seeding temporarily
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // No seeding here → avoids BCrypt issue completely
            base.OnModelCreating(modelBuilder);
        }


        //................................................................//
    }
}

//........................................o0oEND OF FILEo0o.........................................//
