using Marketplace.BAL.DbContext;
using Marketplace.DAL.Dtos.AttributeDto;
using Marketplace.DAL.Dtos.ProductDtos;
using Marketplace.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketplace.DAL.Dtos.ProductVariantDto;
using Marketplace.BAL.Services.AttributeService;
using Marketplace.DAL.Models;
using Marketplace.BAL.Services.ProductService;
using Marketplace.BAL.Services.ImageService;

namespace Marketplace.BAL.Services.ProductVariantService;
public class ProductVariantService(ApplicationDbContext dbContext, IProductService productService, IImageService imageService) : IProductVariantService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IProductService _productService = productService;
    private readonly IImageService _imageService = imageService;

    public async Task<ServiceResponse<ProductResponseDto>> AddProductVariant(AddProductVariantDto model, string userId)
    {
        ServiceResponse<ProductResponseDto> serviceResponse = new ServiceResponse<ProductResponseDto>();
        int productId = 0;
        try
        {
            var attribute = await _dbContext.Attributes
                            .Include(attribute => attribute.Product)
                            .FirstOrDefaultAsync(attribute => attribute.AttributeId.Equals(model.AttributeId));

            if (attribute is null || !attribute.Product.UserId.Equals(userId))
            {
                serviceResponse.Message = $"Attribute With Id: {model.AttributeId} Not Found";
                return serviceResponse;
            }

            var newProductVariant = new ProductVariant
            {
                VariantName = model.VariantName,
                AttributeId = model.AttributeId
            };

            if (model.VariantImages is not null && model.VariantImages.Length > 0)
            {
                var imagesList = await _imageService.GetAllProductImagesIds(attribute.ProductId);

                for (var i = 0; i < model.VariantImages.Length; i++)
                {
                    bool found = imagesList.Any(s => s.Contains(model.VariantImages[i]));

                    if (!found)
                    {
                        serviceResponse.Message = "This Image Id Is Not In This Product Images, Please Add It First";
                        return serviceResponse;
                    }
                }

                newProductVariant.VariantImages = model.VariantImages;
            }

            await _dbContext.ProductVariants.AddAsync(newProductVariant);

            await _dbContext.SaveChangesAsync();

            productId = attribute.ProductId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        return await _productService.GetUserProductById(productId, userId);

    }

    public async Task<ServiceResponse<ProductResponseDto>> UpdateProductVariant(UpdateProductVariantDto model, string userId)
    {
        ServiceResponse<ProductResponseDto> serviceResponse = new ServiceResponse<ProductResponseDto>();
        int productId = 0;
        try
        {
            var variant = await _dbContext.ProductVariants
            .Include(variant => variant.Attribute)
            .ThenInclude(attribute => attribute.Product)
            .FirstOrDefaultAsync(variant => variant.VariantId.Equals(model.VariantId));

            if (variant is null || !variant.Attribute.Product.UserId.Equals(userId))
            {
                serviceResponse.Message = $"Variant With Id: {model.VariantId} Not Found";
                return serviceResponse;
            }

            variant.VariantName = model.VariantName;

            if (model.VariantImages is not null && model.VariantImages.Length > 0)
            {
                var imagesList = await _imageService.GetAllProductImagesIds(variant.Attribute.ProductId);

                for (var i = 0; i < model.VariantImages.Length; i++)
                {
                    bool found = imagesList.Any(s => s.Contains(model.VariantImages[i]));

                    if (!found)
                    {
                        serviceResponse.Message = "This Image Id Is Not In This Product Images, Please Add It First";
                        return serviceResponse;
                    }
                }

                variant.VariantImages = model.VariantImages;
            }

            _dbContext.ProductVariants.Update(variant);
            await _dbContext.SaveChangesAsync();

            productId = variant.Attribute.ProductId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        return await _productService.GetUserProductById(productId, userId);
    }

    public async Task<ServiceResponse<List<ProductResponseDto>>> DeleteProductVariant(int variantId, string userId)
    {
        ServiceResponse<List<ProductResponseDto>> serviceResponse = new ServiceResponse<List<ProductResponseDto>>();

        try
        {
            var variant = await _dbContext.ProductVariants
                .Include(variant => variant.Attribute)
                .ThenInclude(attribute => attribute.Product)
                .FirstOrDefaultAsync(variant => variant.VariantId.Equals(variantId));

            if (variant is null || !variant.Attribute.Product.UserId.Equals(userId))
            {
                serviceResponse.Message = $"Variant With Id: {variantId} Not Found";
                return serviceResponse;
            }

            _dbContext.ProductVariants.Remove(variant);

            await dbContext.SaveChangesAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        return await _productService.GetAllUserProducts(userId, null, null, null, 1, 5);
    }

}
