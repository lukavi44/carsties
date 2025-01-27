using System;

namespace AuctionService.Entities;

public class Auction
{
    public Guid Id {get; set; }
    public int ReservePrice {get; set; } = 0;
    public string Seller { get; set; }
    public string Winner { get; set; }
    public int? SoldAmount { get; set; }
    public int? CurrentHighestBid { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // postgres insists on Utc
    public DateTime UpdatedAt { get; set; }
    public DateTime AuctionEnd { get; set; }
    public Status Status { get; set; } 
    public Item Item { get; set; }
}
