﻿
using Microservices.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.ProductAPI.DbContexts;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasData(new Product
        {
            ProductId = Guid.NewGuid(),
            Name = "Samosa",
            Price = 15,
            Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://microservicesimgs.blob.core.windows.net/microservices/14.jpg",
            CategoryName = "Appetizer"
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            ProductId = Guid.NewGuid(),
            Name = "Paneer Tikka",
            Price = 13.99,
            Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://microservicesimgs.blob.core.windows.net/microservices/12.jpg",
            CategoryName = "Appetizer"
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            ProductId = Guid.NewGuid(),
            Name = "Sweet Pie",
            Price = 10.99,
            Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://microservicesimgs.blob.core.windows.net/microservices/11.jpg",
            CategoryName = "Dessert"
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            ProductId = Guid.NewGuid(),
            Name = "Pav Bhaji",
            Price = 15,
            Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://microservicesimgs.blob.core.windows.net/microservices/13.jpg",
            CategoryName = "Entree"
        });
    }
}
