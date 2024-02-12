using AutoMapper;
using Marketplace.BAL.Dtos.AttributeDto;
using Marketplace.BAL.Dtos.ProductVariantDto;
using Marketplace.DAL.Models;

namespace Marketplace.BAL.Maps;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.ProductAttributes, src => src.MapFrom(src => src.ProductAttributes)).ReverseMap();

        CreateMap<CreateAttributeDto, ProductAttribute>()
            .ForMember(dest => dest.ProductVariants, opt => opt.MapFrom(src => src.ProductVariants)).ReverseMap();

        CreateMap<CreateProductVariantDto, ProductVariant>().ReverseMap();



        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.ProductAttributes, src => src.MapFrom(src => src.ProductAttributes)).ReverseMap();

        CreateMap<UpdateAttributeDto, ProductAttribute>()
            .ForMember(dest => dest.ProductVariants, src => src.MapFrom(src => src.ProductVariants)).ReverseMap();

        CreateMap<UpdateProductVariantDto, ProductVariant>().ReverseMap();
    }
}
