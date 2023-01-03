using System;
using CrossFitWOD.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace CrossFitWOD.Data
{
    public class AppDBContext: IdentityDbContext<Models.User>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public virtual DbSet<WorkoutOfTheDay>? WODs { get; set; }
        public virtual DbSet<StrengthTraining>? StrengthTrainingWorkouts { get; set; }
        public virtual DbSet<Exercise>? Exercises { get; set; }


        // Provides the ability for us to manage the table properties of the tables in the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<WorkoutOfTheDay>()
                .ToTable("CrossFit","Workouts")
                .HasOne(p => p.User)
                .WithMany(b => b.Workouts)
                .IsRequired();

            modelBuilder.Entity<StrengthTraining>()
                .ToTable("Strength", "Workouts");

            modelBuilder.Entity<Exercise>()
                .ToTable("Exercises", "Workouts");

            modelBuilder.Entity<User>()
            .HasMany(c => c.Workouts)
            .WithOne(e => e.User).IsRequired();


        }
    }
}

