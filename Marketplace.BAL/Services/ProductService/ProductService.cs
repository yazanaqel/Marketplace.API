using Marketplace.BAL.DbContext;
using Marketplace.DAL;
using Marketplace.DAL.Dtos.ProductDtos;
using Microsoft.EntityFrameworkCore;
using Marketplace.DAL.Models;
using Microsoft.AspNetCore.Http;
using Marketplace.BAL.Services.ImageService;
using Marketplace.DAL.Dtos.AttributeDto;
using Marketplace.DAL.Dtos.ProductVariantDto;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;

namespace Marketplace.BAL.Services.ProductService;
public class ProductService(ApplicationDbContext dbContext, IImageService imageService) : IProductService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IImageService _imageService = imageService;

    public async Task<ServiceResponse<List<ProductResponseDto>>> GetAllProducts(string? sortColumn, string? sortOrder, string? searchItem, int page, int pageSize)
    {
        ServiceResponse<List<ProductResponseDto>> serviceResponse = new ServiceResponse<List<ProductResponseDto>>();

        var response = new List<ProductResponseDto>();

        try
        {
            IQueryable<Product> productsQuery = _dbContext.Products
            .Include(attribute => attribute.Attributes)
            .ThenInclude(variant => variant.ProductVariants);

            if (!productsQuery.Any())
            {
                serviceResponse.Message = "No Products Found :(";
                return serviceResponse;
            }

            if (!string.IsNullOrWhiteSpace(searchItem))
            {
                productsQuery = productsQuery.Where(
                    x => x.ProductName.Contains(searchItem));
            }


            if (sortOrder?.ToLower() == "desc")
            {
                productsQuery = productsQuery.OrderByDescending(GetSortProperty(sortColumn));
            }
            else
            {
                productsQuery = productsQuery.OrderBy(GetSortProperty(sortColumn));
            }

            var products = await productsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();


            foreach (var product in products)
            {
                var productResponseDto = new ProductResponseDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    ProductMainImage = product.ProductMainImage,
                    Description = product.Description,
                    ProductImages = await _imageService.GetAllProductImagesIds(product.ProductId),
                };


                if (product.Attributes is not null && product.Attributes.Count > 0)
                {
                    productResponseDto.AttributesList = new List<AttributeResponseDto>();

                    foreach (var attribute in product.Attributes)
                    {
                        var attributeResponseDto = new AttributeResponseDto
                        {
                            AttributeId = attribute.AttributeId,
                            AttributeName = attribute.AttributeName,
                            ProductId = attribute.ProductId,
                        };

                        if (attribute.ProductVariants is not null && attribute.ProductVariants.Count > 0)
                        {
                            var productVariantResponseDto = new List<ProductVariantResponseDto>();

                            foreach (var variant in attribute.ProductVariants)
                            {
                                productVariantResponseDto.Add(new ProductVariantResponseDto
                                {
                                    VariantId = variant.VariantId,
                                    VariantName = variant.VariantName,
                                    VariantImages = variant.VariantImages,
                                    AttributeId = variant.AttributeId,
                                });
                            }

                            attributeResponseDto.ProductVariantList = productVariantResponseDto;
                        }

                        productResponseDto.AttributesList.Add(attributeResponseDto);
                    }
                }

                response.Add(productResponseDto);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        serviceResponse.Data = response;
        serviceResponse.Message = "Successful operation :)";
        serviceResponse.Success = true;

        return serviceResponse;

    }

    public async Task<ServiceResponse<List<ProductResponseDto>>> GetAllUserProducts(string userId, string? sortColumn, string? sortOrder, string? searchItem, int page, int pageSize)
    {
        ServiceResponse<List<ProductResponseDto>> serviceResponse = new ServiceResponse<List<ProductResponseDto>>();

        IQueryable<Product> productsQuery = _dbContext.Products
                    .Include(attribute => attribute.Attributes)
                    .ThenInclude(variant => variant.ProductVariants)
                    .Where(product => product.UserId.Equals(userId));

        if (!productsQuery.Any())
        {
            serviceResponse.Message = "You Dont Have Any Products!";
            return serviceResponse;
        }

        if (!string.IsNullOrWhiteSpace(searchItem))
        {
            productsQuery = productsQuery.Where(
                x => x.ProductName.Contains(searchItem));
        }

        if (sortOrder?.ToLower() == "desc")
        {
            productsQuery = productsQuery.OrderByDescending(GetSortProperty(sortColumn));
        }
        else
        {
            productsQuery = productsQuery.OrderBy(GetSortProperty(sortColumn));
        }

        var userProducts = await productsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();


        var response = new List<ProductResponseDto>();

        try
        {

            foreach (var product in userProducts)
            {
                var productResponseDto = new ProductResponseDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    ProductMainImage = product.ProductMainImage,
                    Description = product.Description,
                    ProductImages = await _imageService.GetAllProductImagesIds(product.ProductId),
                };


                if (product.Attributes is not null && product.Attributes.Count > 0)
                {
                    productResponseDto.AttributesList = new List<AttributeResponseDto>();

                    foreach (var attribute in product.Attributes)
                    {
                        var attributeResponseDto = new AttributeResponseDto
                        {
                            AttributeId = attribute.AttributeId,
                            AttributeName = attribute.AttributeName,
                            ProductId = attribute.ProductId,
                        };

                        if (attribute.ProductVariants is not null && attribute.ProductVariants.Count > 0)
                        {
                            var productVariantResponseDto = new List<ProductVariantResponseDto>();

                            foreach (var variant in attribute.ProductVariants)
                            {
                                productVariantResponseDto.Add(new ProductVariantResponseDto
                                {
                                    VariantId = variant.VariantId,
                                    VariantName = variant.VariantName,
                                    VariantImages = variant.VariantImages,
                                    AttributeId = variant.AttributeId,
                                });
                            }

                            attributeResponseDto.ProductVariantList = productVariantResponseDto;
                        }

                        productResponseDto.AttributesList.Add(attributeResponseDto);
                    }
                }

                response.Add(productResponseDto);
            }

            response.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        serviceResponse.Data = response;
        serviceResponse.Message = "Successful operation :)";
        serviceResponse.Success = true;

        return serviceResponse;

    }

    public async Task<ServiceResponse<ProductResponseDto>> GetUserProductById(int productId, string userId)
    {
        ServiceResponse<ProductResponseDto> serviceResponse = new ServiceResponse<ProductResponseDto>();

        var product = await _dbContext.Products
             .Include(attribute => attribute.Attributes)
            .ThenInclude(variant => variant.ProductVariants)
            .Where(product => product.UserId.Equals(userId))
            .FirstOrDefaultAsync(x => x.ProductId.Equals(productId));

        if (product is null)
        {
            serviceResponse.Message = $"Product With Id: {productId} Not Found";
            return serviceResponse;
        }

        try
        {

            var response = new ProductResponseDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                ProductMainImage = product.ProductMainImage,
                Description = product.Description,
                ProductImages = await _imageService.GetAllProductImagesIds(product.ProductId)
            };

            if (product.Attributes is not null && product.Attributes.Count > 0)
            {
                response.AttributesList = new List<AttributeResponseDto>();

                foreach (var attribute in product.Attributes)
                {
                    var attributeResponseDto = new AttributeResponseDto
                    {
                        AttributeId = attribute.AttributeId,
                        AttributeName = attribute.AttributeName,
                        ProductId = attribute.ProductId,
                    };

                    if (attribute.ProductVariants is not null && attribute.ProductVariants.Count > 0)
                    {
                        var productVariantResponseDto = new List<ProductVariantResponseDto>();

                        foreach (var variant in attribute.ProductVariants)
                        {
                            productVariantResponseDto.Add(new ProductVariantResponseDto
                            {
                                VariantId = variant.VariantId,
                                VariantName = variant.VariantName,
                                VariantImages = variant.VariantImages,
                                AttributeId = variant.AttributeId,
                            });
                        }

                        attributeResponseDto.ProductVariantList = productVariantResponseDto;
                    }

                    response.AttributesList.Add(attributeResponseDto);
                }
            }

            serviceResponse.Data = response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        serviceResponse.Message = "Successful operation :)";
        serviceResponse.Success = true;

        return serviceResponse;

    }

    public async Task<ServiceResponse<ProductResponseDto>> AddProduct(AddProductDto model, string userId)
    {
        var newProduct = new Product();

        try
        {
            newProduct.ProductName = model.ProductName;
            newProduct.Price = model.Price;
            newProduct.UserId = userId;
            newProduct.Description = model.Description;

            _dbContext.Products.Add(newProduct);
            await _dbContext.SaveChangesAsync();

            if (model.Images is not null)
            {
                await UploadProductImages(model.Images, newProduct.ProductId);

                var productImages = await _imageService.GetAllProductImagesIds(newProduct.ProductId);

                var product = await _dbContext.Products
                 .FirstOrDefaultAsync(x => x.ProductId.Equals(newProduct.ProductId));

                product.ProductMainImage = productImages.FirstOrDefault();

                _dbContext.Products.Update(newProduct);
                await _dbContext.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        return await GetUserProductById(newProduct.ProductId, userId);

    }

    public async Task<ServiceResponse<ProductResponseDto>> UpdateProduct(UpdateProductDto model, string userId)
    {
        ServiceResponse<ProductResponseDto> serviceResponse = new ServiceResponse<ProductResponseDto>();

        try
        {

            var product = await _dbContext.Products
                             .Where(product => product.UserId.Equals(userId))
                             .FirstOrDefaultAsync(x => x.ProductId.Equals(model.ProductId));

            if (product is null)
            {
                serviceResponse.Message = $"Product With Id: {model.ProductId} Not Found";
                return serviceResponse;
            }

            var imagesList = await _imageService.GetAllProductImagesIds(model.ProductId);

            if (model.ProductMainImage is not null)
            {
                bool found = imagesList.Any(s => s.Contains(model.ProductMainImage));

                if (!found)
                {
                    serviceResponse.Message = "This Image Id Is Not In This Product Images, Please Add It First";
                    return serviceResponse;
                }
            }

            product.ProductMainImage = model.ProductMainImage;
            product.ProductName = model.ProductName;
            product.Price = model.Price;
            product.Description = model.Description;

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();

            if (model.Images is not null)
            {
                await UploadProductImages(model.Images, product.ProductId);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        return await GetUserProductById(model.ProductId, userId);
    }

    public async Task<ServiceResponse<List<ProductResponseDto>>> DeleteProduct(int productId, string userId)
    {
        ServiceResponse<List<ProductResponseDto>> serviceResponse = new ServiceResponse<List<ProductResponseDto>>();
        try
        {
            var product = await _dbContext.Products
                             .Where(product => product.UserId.Equals(userId))
                             .FirstOrDefaultAsync(x => x.ProductId.Equals(productId));

            if (product is null)
            {
                serviceResponse.Message = $"Product With Id: {productId} Not Found";
                return serviceResponse;
            }

            _dbContext.Products.Remove(product);

            await dbContext.SaveChangesAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        return await GetAllUserProducts(userId, null, null, null, 1, 5);
    }

    private async Task UploadProductImages(IFormFile[] images, int newProductId)
    {

        await _imageService.Process(images.Select(i => new ImageInputModel
        {
            Name = i.FileName,
            Type = i.ContentType,
            Content = i.OpenReadStream(),
        }), newProductId);

    }
    private static Expression<Func<Product, object>> GetSortProperty(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "name" => product => product.ProductName,
            "price" => product => product.Price,
            _ => product => product.ProductId
        };
    }

}
