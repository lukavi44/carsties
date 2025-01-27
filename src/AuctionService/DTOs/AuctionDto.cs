using System;

namespace AuctionService.DTOs;

public class AuctionDto
{
    public Guid Id {get; set; }
    public int ReservePrice {get; set; }
    public string Seller { get; set; }
    public string Winner { get; set; }
    public int SoldAmount { get; set; }
    public int CurrentHighestBid { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime AuctionEnd { get; set; }
    public string Status { get; set; } //not enum but string,
    // because we want to return the value of the enum, not the database value which was just an integer
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public int MileAge { get; set; }
    public string ImageUrl { get; set; }
}

//no return values in DTOs, it should be collection of properties