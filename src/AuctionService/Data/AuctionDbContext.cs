using System;
using AuctionService.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Auction> Auctions { get; set; } /*
    This is a property that represents a collection of Auction entities.
    DbSet<Auction> allows Entity Framework Core to track and query the Auction entities in the database.
    DbSet is essentially a table in your database, and each Auction is a row in that table. */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
        /*This will need a migration and will create 3 tables in a database.*/
    }
}
