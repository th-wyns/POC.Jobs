using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace POC.Jobs.Storage.EntityFrameworkCore
{
    class JobStateDbContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public JobStateDbContext(DbContextOptions<JobStateDbContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            this.Database.EnsureCreated();
            // TODO: add migration to create tables in existing DBs as well
            //this.Database.Migrate();
        }

        public DbSet<JobState> JobStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobState>().Property(e => e.Id).ValueGeneratedOnAdd();
            //modelBuilder.Entity<JobState>().Property(e => e.Status).HasConversion<string>();
            modelBuilder.Entity<JobState>().HasKey(e => e.Id);
            modelBuilder.Entity<JobState>().HasIndex(e => e.ProjectId);
            modelBuilder.Entity<JobState>().HasIndex(e => e.OwnerId);
            modelBuilder.Entity<JobState>().HasIndex(e => e.Status);
            modelBuilder.Entity<JobState>().HasIndex(e => e.Type);
            modelBuilder.Entity<JobState>().HasIndex(e => e.Results);
            modelBuilder.Entity<JobState>().HasIndex(e => e.Errors);
            modelBuilder.Entity<JobState>().HasIndex(e => e.Total);
            modelBuilder.Entity<JobState>().ToTable("JobState");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var connectionString = Configuration.ConnectionString;
            // TODO: remove after testing
            System.Console.WriteLine($"JobStateDbContext ConnectionString: {connectionString}");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
