using AutoMapper;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Brands;

public class BrandMappingProfile : Profile
{
    public BrandMappingProfile()
    {
        CreateMap<Brand, BrandCreateDto>();
        CreateMap<Brand, BrandDto>();
    }
}
