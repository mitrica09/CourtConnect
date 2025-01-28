using CourtConnect.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CourtConnect.StartPackage.Database
{
    public class CourtConnectDbContext :DbContext
    {
        public CourtConnectDbContext(DbContextOptions<CourtConnectDbContext> options)
       : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Club> Clubs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Relationship User
            modelBuilder.Entity<User>()
                .HasOne(b => b.Level) 
                .WithMany(a => a.Users) 
                .HasForeignKey(b => b.LevelId);

            modelBuilder.Entity<User>()
               .HasOne(b => b.Club)
               .WithMany(a => a.Users)
               .HasForeignKey(b => b.ClubId);
            #endregion

        }
    }
}
