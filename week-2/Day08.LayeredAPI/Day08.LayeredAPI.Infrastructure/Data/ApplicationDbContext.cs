using Microsoft.EntityFrameworkCore;
using Day08.LayeredAPI.Domain.Entities;

namespace Day08.LayeredAPI.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Employee configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Salary).HasPrecision(18, 2);

            // Relationship
            entity.HasOne(e => e.Department)
                  .WithMany(d => d.Employees)
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Department configuration
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Location).IsRequired().HasMaxLength(200);
        });

        // User configuration (Day 10: JWT Authentication)
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
            entity.HasIndex(u => u.Username).IsUnique();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
        });

        // Seed data
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Engineering", Description = "Software Development", Location = "Building A" },
            new Department { Id = 2, Name = "Human Resources", Description = "HR Operations", Location = "Building B" },
            new Department { Id = 3, Name = "Marketing", Description = "Marketing and Sales", Location = "Building C" }
        );

        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@company.com",
                PhoneNumber = "+1234567890",
                DepartmentId = 1,
                Position = "Senior Developer",
                Salary = 95000,
                HireDate = new DateTime(2020, 1, 15),
                IsActive = true
            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@company.com",
                PhoneNumber = "+1234567891",
                DepartmentId = 2,
                Position = "HR Manager",
                Salary = 85000,
                HireDate = new DateTime(2019, 6, 10),
                IsActive = true
            }
        );

        // User seed data (Day 10: JWT Authentication)
        // Password for both users is "Password123!" (hashed with BCrypt)
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@company.com",
                PasswordHash = "$2a$11$BczJ7KhXrF.rWEn8/aBY8.pjVS6KhUbG5xkTZJCZq6FRqXMPvMH7K", // Password123!
                Role = "Admin",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = 2,
                Username = "user",
                Email = "user@company.com",
                PasswordHash = "$2a$11$BczJ7KhXrF.rWEn8/aBY8.pjVS6KhUbG5xkTZJCZq6FRqXMPvMH7K", // Password123!
                Role = "User",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
