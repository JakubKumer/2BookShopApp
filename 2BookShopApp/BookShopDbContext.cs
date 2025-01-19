using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;



namespace _2BookShopApp
{
    internal class BookShopDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-3FFUDQC\SQLEXPRESS;Initial Catalog=BookShopDb;Integrated Security=True"); // Zastąp to swoim łańcuchem połączenia
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().HasData(
        new Book
        {
            BookId = 1,
            Title = "Batman",
            Author = "Christopher Nolan",
            PublicationYear = 2008,
            Price = (decimal)49.99
        },
        new Book
        {
            BookId = 2,
            Title = "Marvel",
            Author = "Anonym",
            PublicationYear = 2010,
            Price = (decimal)59.99
        }
    );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "Mati",
                    Password = "Mati",
                    Name = "Mateusz",
                    Surname = "Jakis",
                    IsEmployee = false
                }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    UserId = 2, 
                    Username = "Admin",
                    Password = "Admin",
                    Name = "Jakub",
                    Surname = "Kumer",
                    IsEmployee = true,
                    type = Type.Pracownik
                }
            );
        }
    }
}
