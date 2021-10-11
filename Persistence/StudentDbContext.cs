using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence
{
    public class StudentDbContext : IdentityDbContext<AppUser>
    {
        public string DbPath { get; private set; }

        public StudentDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}sample3.db";
        }

        public StudentDbContext(DbContextOptions<StudentDbContext> options)
            : base(options)
        {
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath(folder);
            //DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}sample3.db";
        }


        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            modelBuilder.Entity<Student>(b =>
            {
                b.ToTable(nameof(Student));

                b.HasIndex(e => e.UserName).IsUnique();
                
                b.Property(p => p.UserName).IsRequired().HasMaxLength(20);
                b.Property(p => p.FirstName).IsRequired().HasMaxLength(20);
                b.Property(p => p.LastName).IsRequired().HasMaxLength(20);
                b.Property(p => p.Career).IsRequired().HasMaxLength(50);
                b.Property(p => p.Age).IsRequired();
            });
        }
    }
}