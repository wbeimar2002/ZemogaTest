using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ZemogaTest.Utilities.Dtos;
using ZemogaTest.Utilities.Entities;

namespace ZemogaTest.Repositories
{
    public partial class ZemogaTestContext : DbContext
    {
        public ZemogaTestContext()
        {
            
        }
        public ZemogaTestContext(DbContextOptions<ZemogaTestContext> options) : base(options)
        {

        }

        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Rol> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=dbZemogaTest;Trusted_Connection=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.StatusId);
                entity.ToTable("Status");
                entity.Property(e => e.Description).HasMaxLength(50);
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.RolId);
                entity.ToTable("Rol");
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.ToTable("User");
                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.HasKey(e => e.BlogId);
                entity.ToTable("Blog");
                entity.Property(e => e.Title).HasMaxLength(50);
            });


            // Seeding information 
            modelBuilder.Entity<Status>().HasData(new Status { StatusId = 1, Description = "Created", CreatedAt = System.DateTime.Now, CreatedBy="Admin" });
            modelBuilder.Entity<Status>().HasData(new Status { StatusId = 2, Description = "Approved", CreatedAt = System.DateTime.Now, CreatedBy = "Admin" });
            modelBuilder.Entity<Status>().HasData(new Status { StatusId = 3, Description = "Rejected", CreatedAt = System.DateTime.Now, CreatedBy = "Admin" });
            modelBuilder.Entity<Status>().HasData(new Status { StatusId = 4, Description = "Deleted", CreatedAt = System.DateTime.Now, CreatedBy = "Admin" });
            modelBuilder.Entity<Status>().HasData(new Status { StatusId = 5, Description = "PendingToApproval", CreatedAt = System.DateTime.Now, CreatedBy = "Admin" });

            modelBuilder.Entity<Rol>().HasData(new Rol { RolId = 1, Name = "writer", CreatedAt = System.DateTime.Now, CreatedBy = "Admin" });
            modelBuilder.Entity<Rol>().HasData(new Rol { RolId = 2, Name = "editor", CreatedAt = System.DateTime.Now, CreatedBy = "Admin" });

            modelBuilder.Entity<User>().HasData(new User { UserId = 1, UserName = "walex", Password ="123" , RolId=1, FullName= "Alexander Gonzalez", CreatedAt = System.DateTime.Now, CreatedBy = "Admin" });
            modelBuilder.Entity<User>().HasData(new User { UserId = 2, UserName = "naty", Password = "123", RolId = 2, FullName = "Natalia Munera", CreatedAt = System.DateTime.Now, CreatedBy = "Admin" });
        }


    }
}
