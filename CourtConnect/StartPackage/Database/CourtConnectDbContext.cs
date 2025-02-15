using CourtConnect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CourtConnect.StartPackage.Database
{
    public class CourtConnectDbContext : IdentityDbContext<User>
    {
        public CourtConnectDbContext(DbContextOptions<CourtConnectDbContext> options) : base(options)
        {
        }
        
        public DbSet<Level> Levels { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Announce> Announces { get; set; }
        public DbSet<AnnounceStatus> AnnouncesStatus { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<SetResult> SetsResult { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<UserMatch> UserMatches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            #region Relationship Ranking
            modelBuilder.Entity<User>()
            .HasOne(b => b.Ranking)
            .WithOne(a => a.User)
            .HasForeignKey<Ranking>(b => b.UserId);
            #endregion

            #region Relationship Court
            modelBuilder.Entity<Court>()
            .HasOne(b => b.Location)
            .WithMany(a => a.Courts)
            .HasForeignKey(b => b.LocationId);
            #endregion

            #region Relationship Match
            modelBuilder.Entity<Match>()
            .HasOne(b => b.Status)
            .WithMany(a => a.Matches)
            .HasForeignKey(b => b.StatusId);

            modelBuilder.Entity<Match>()
            .HasOne(b => b.Result)
            .WithMany(a => a.Matches)
            .HasForeignKey(b => b.ResultId);
            #endregion

            #region Relationship UserMatch
            modelBuilder.Entity<UserMatch>()
            .HasOne(b => b.User)
            .WithMany(a => a.UserMatches)
            .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<UserMatch>()
            .HasOne(b => b.Match)
            .WithMany(a => a.UserMatches)
            .HasForeignKey(b => b.MatchId);
            #endregion

            #region Relationship SetResult
            modelBuilder.Entity<SetResult>()
            .HasOne(b => b.Set)
            .WithMany(a => a.SetResults)
            .HasForeignKey(b => b.SetId);

            modelBuilder.Entity<SetResult>()
            .HasOne(b => b.Match)
            .WithMany(a => a.SetResults)
            .HasForeignKey(b => b.MatchId);

            modelBuilder.Entity<SetResult>()
            .HasOne(b => b.User)
            .WithMany(a => a.SetResults)
            .HasForeignKey(b => b.UserId);
            #endregion

            #region Relationship Match
            modelBuilder.Entity<Announce>()
            .HasOne(b => b.User)
            .WithMany(a => a.Announces)
            .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Announce>()
            .HasOne(b => b.Court)
            .WithMany(a => a.Announces)
            .HasForeignKey(b => b.CourtId);

            modelBuilder.Entity<Announce>()
            .HasOne(b => b.AnnounceStatus)
            .WithMany(a => a.Announces)
            .HasForeignKey(b => b.AnnounceStatusId);
            #endregion


        }
    }
}
