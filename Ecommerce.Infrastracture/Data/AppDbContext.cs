using Ecommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data
{
    public  class AppDbContext :DbContext
    {
       // private readonly DbContextOptions<AppDbContext> options;

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
          //  this.options = options;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          //  optionsBuilder.UseSqlServer("Server=.;Database=EcommerceAPI2;Trusted_Connection=True;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>().HasKey(
                x => new {
                x.Id,
                x.ProductId,
                x.OrderId,
                });


            modelBuilder.Entity<Categories>().HasData(
                new Categories { Id = 1, Name = "Electronics", Description = "Devices and gadgets" },
                new Categories { Id = 2, Name = "Books", Description = "Books and literature" },
                new Categories { Id = 3, Name = "Clothing", Description = "Apparel and accessories" }
           );

            modelBuilder.Entity<LocalUser>().HasData(
                new LocalUser { Id = 1, UserName = "Haggag281", Password = "password123", Role = "Admin" },
                new LocalUser { Id = 2, UserName = "Tarek777", Password = "password456", Role = "User" }
            );

            modelBuilder.Entity<Products>().HasData(
                new Products { Id = 1, Name = "Smartphone", Price = 299, Image = "smartphone.jpg", CategoryId = 1 },
                new Products { Id = 2, Name = "Laptop", Price = 799, Image = "laptop.jpg", CategoryId = 1 },
                new Products { Id = 3, Name = "Novel", Price = 19, Image = "novel.jpg", CategoryId = 2 },
                new Products { Id = 4, Name = "T-Shirt", Price = 9, Image = "tshirt.jpg", CategoryId = 3 },
                new Products { Id = 5, Name = "Jeans", Price = 49, Image = "jeans.jpg", CategoryId = 3 }
            );

            modelBuilder.Entity<Orders>().HasData(
                new Orders { Id = 1, OrderStatus = "Pending", OrderDate = new DateTime(2023, 12, 11), LocalUserId = 1 },
                new Orders { Id = 2, OrderStatus = "Completed", OrderDate = new DateTime(2023, 12, 12), LocalUserId = 2 },
                new Orders { Id = 3, OrderStatus = "Shipped", OrderDate = new DateTime(2023, 12, 13), LocalUserId = 1 }
            );

            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail { Id = 1, OrderId = 1, ProductId = 1, Price = 299, Quantity = 1 },
                new OrderDetail { Id = 2, OrderId = 1, ProductId = 4, Price = 9, Quantity = 2 },
                new OrderDetail { Id = 3, OrderId = 2, ProductId = 3, Price = 19, Quantity = 1 },
                new OrderDetail { Id = 4, OrderId = 3, ProductId = 2, Price = 799, Quantity = 1 },
                new OrderDetail { Id = 5, OrderId = 3, ProductId = 5, Price = 9, Quantity = 1 }
            );

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<LocalUser> LocalUser { get; set; }
        public DbSet<Categories> Categories { get; set; }
        
    }
}
