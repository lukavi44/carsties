using System;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;

    public AuctionsController(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    /*This code is a controller action in an ASP.NET Core API that retrieves all auctions from the database,
    includes related item data, orders the auctions by the Make property of the associated Item,
    and then maps the result to a list of AuctionDto objects before returning it to the client.*/
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
    {
        var auctions = await _context.Auctions.Include(x => x.Item).OrderBy(x => x.Item.Make).ToListAsync();

        return _mapper.Map<List<AuctionDto>>(auctions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id) 
    {
        var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null) return NotFound();

        return _mapper.Map<AuctionDto>(auction);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
    {
        var auction = _mapper.Map<Auction>(auctionDto);
        //TODO: add current user as seller; check the username first
        auction.Seller = "test";

        _context.Auctions.Add(auction);
        //Entity is tracking this (auction) in memory

        var result = await _context.SaveChangesAsync() > 0; /* grater than zero because this SaveChangesAsync method
        returns an integer for each change it was able to save in the database.
        If it returns zero, that means nothing was saved into our database and we know our result is going
        to be false. But if the changes were more than zero, then we can presume that was successful and this will evaluate to true.
        */

        if (!result) return BadRequest("Could not save changes to the DB");

        return CreatedAtAction(nameof(GetAuctionById), new {auction.Id}, _mapper.Map<AuctionDto>(auction));
        /* 
        And this particular method, this takes an argument of the Guid of the auction so we can specify as
        a second parameter in the created at action, we can specify new and then we can simply specify the
        auction ID as the parameter that's needed for this particular action or endpoint.
        And then as a third parameter, we can return the auction data.
        So in order to return an auction data from this, we need to go from our auction entity into an auction
        DTO So once again, we'll utilize mapper functionality for this and we'll map into an auction DTO from the auction.
        */
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        var auction = await _context.Auctions.Include(x => x.Item)
        .FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null) return NotFound();

        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.MileAge = updateAuctionDto.MileAge ?? auction.Item.MileAge;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Ok();

        return BadRequest("Problem when saving changes");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await _context.Auctions.FindAsync(id);

        if (auction == null) return NotFound();

        _context.Auctions.Remove(auction);

        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Problem when saving changes");

        return Ok();
    }
}

/*The way that dependency injection works is that when our framework creates a new instance of the
auctions controller, which it will do when it receives a request into this particular route, then
it's going to take a look at the arguments inside the controller and it's going to say, Right, okay,
I see you want a dbcontext and a mapper and it's going to instantiate these classes and make them available
inside here.*/