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

        // Provides the ability for us to manage the table properties of the tables in the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

