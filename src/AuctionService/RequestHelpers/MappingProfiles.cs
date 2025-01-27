using System;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;

namespace AuctionService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item); //Maps Auction (the entity) to AuctionDto (the DTO).
        //The IncludeMembers call indicates that the properties of the related Item entity should also be included in the mapping process.

        /*Example:
            If Auction contains an Item object, 
            AutoMapper will automatically map properties from Item to AuctionDto. */

        CreateMap<Item, AuctionDto>();  //Maps Item (an associated entity) directly to AuctionDto.
        //This ensures that if AutoMapper encounters an Item object, it knows how to map it to AuctionDto.

        CreateMap<CreateAuctionDto, Auction>()
            .ForMember(d => d.Item, o => o.MapFrom(s => s));
            /*Maps CreateAuctionDto (likely used for incoming API requests) to the Auction entity.
            The ForMember method specifies that the Item property of Auction should be populated using the CreateAuctionDto object itself.
            o.MapFrom(s => s) tells AutoMapper to map the CreateAuctionDto object as a whole to the Item property. */

        CreateMap<CreateAuctionDto, Item>(); 
        //Maps CreateAuctionDto directly to the Item entity, which is useful when CreateAuctionDto has fields that correspond to Item.
    }
}
