using AutoMapper;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Ratings;

public class RatingMappingProfile : Profile
{
    public RatingMappingProfile()
    {
        CreateMap<ProductRating, ProductRatingDto>();
        CreateMap<ProductRating, ProductRatingCreateDto>()
            .ForMember(
                dest => dest.ProductSlug,
                opt => opt.MapFrom(src => src.Product.Slug));
    }
}
