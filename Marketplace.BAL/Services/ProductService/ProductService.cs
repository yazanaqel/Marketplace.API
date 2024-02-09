namespace Marketplace.BAL.Services.ProductService;
public class ProductService(ApplicationDbContext dbContext, IImageService imageService) : IProductService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IImageService _imageService = imageService;

    public async Task<ServiceResponse<List<ProductsResponseDto>>> GetAllProducts(string? sortColumn, string? sortOrder, string? searchItem, int page, int pageSize)
    {
        ServiceResponse<List<ProductsResponseDto>> serviceResponse = new ServiceResponse<List<ProductsResponseDto>>();

        try
        {
            IQueryable<Product> productsQuery = _dbContext.Products;

            if (!productsQuery.Any())
            {
                serviceResponse.Message = CustomConstants.NotFound.NoProducts;
                return serviceResponse;
            }

            if (!string.IsNullOrWhiteSpace(searchItem))
            {
                productsQuery = productsQuery.Where(
                    x => x.ProductName.Contains(searchItem));
            }


            if (sortOrder?.ToLower() == CustomConstants.SortOrder.Descinding.ToLower())
            {
                productsQuery = productsQuery.OrderByDescending(GetSortProperty(sortColumn));
            }
            else
            {
                productsQuery = productsQuery.OrderBy(GetSortProperty(sortColumn));
            }

            var products = await productsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            serviceResponse.Data = new List<ProductsResponseDto>();

            foreach (var product in products)
            {
                var productsResponseDto = new ProductsResponseDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    ProductMainImage = product.ProductMainImage,
                    Description = product.Description,
                };

                serviceResponse.Data.Add(productsResponseDto);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

        }

        serviceResponse.Message = CustomConstants.Operation.Successful;
        serviceResponse.Success = true;

        return serviceResponse;

    }
    public async Task<ServiceResponse<ProductResponseDto>> GetProductDetails(int productId)
    {

        ServiceResponse<ProductResponseDto> serviceResponse = new ServiceResponse<ProductResponseDto>();

        try
        {
            var product = await _dbContext.Products
            .Where(product => product.ProductId.Equals(productId))
            .Include(attribute => attribute.ProductAttributes)
            .ThenInclude(variant => variant.ProductVariants)
            .SingleOrDefaultAsync();


            if (product is null)
            {
                serviceResponse.Message = CustomConstants.NotFound.Product;
                return serviceResponse;
            }

            serviceResponse.Data = new ProductResponseDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                ProductMainImage = product.ProductMainImage,
                Description = product.Description,
                ProductImages = await _imageService.GetAllProductImagesPaths(productId)
            };

            if (product.ProductAttributes is not null && product.ProductAttributes.Count > 0)
            {
                serviceResponse.Data.ProductAttributesList = new List<AttributeResponseDto>();

                foreach (var attribute in product.ProductAttributes)
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

                    serviceResponse.Data.ProductAttributesList.Add(attributeResponseDto);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        serviceResponse.Message = CustomConstants.Operation.Successful;
        serviceResponse.Success = true;

        return serviceResponse;
    }
    public async Task<ServiceResponse<List<ProductsResponseDto>>> GetAllUserProducts(string userId, string? sortColumn, string? sortOrder, string? searchItem, int page, int pageSize)
    {
        ServiceResponse<List<ProductsResponseDto>> serviceResponse = new ServiceResponse<List<ProductsResponseDto>>();

        try
        {
            IQueryable<Product> productsQuery = _dbContext.Products.Where(product => product.UserId.Equals(userId));

            if (!productsQuery.Any())
            {
                serviceResponse.Message = CustomConstants.NotFound.NoProducts;
                return serviceResponse;
            }

            if (!string.IsNullOrWhiteSpace(searchItem))
            {
                productsQuery = productsQuery.Where(
                    x => x.ProductName.Contains(searchItem));
            }

            if (sortOrder?.ToLower() == CustomConstants.SortOrder.Descinding.ToLower())
            {
                productsQuery = productsQuery.OrderByDescending(GetSortProperty(sortColumn));
            }
            else
            {
                productsQuery = productsQuery.OrderBy(GetSortProperty(sortColumn));
            }

            var userProducts = await productsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();



            serviceResponse.Data = new List<ProductsResponseDto>();


            foreach (var product in userProducts)
            {
                var productsResponseDto = new ProductsResponseDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    ProductMainImage = product.ProductMainImage,
                    Description = product.Description
                };

                serviceResponse.Data.Add(productsResponseDto);
            }

            serviceResponse.Data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

        }

        serviceResponse.Message = CustomConstants.Operation.Successful;
        serviceResponse.Success = true;

        return serviceResponse;

    }

    public async Task<ServiceResponse<ProductResponseDto>> GetUserProductById(int productId, string userId)
    {
        ServiceResponse<ProductResponseDto> serviceResponse = new ServiceResponse<ProductResponseDto>();

        var product = await _dbContext.Products
            .Where(product => product.ProductId.Equals(productId) && product.UserId.Equals(userId))
            .Include(attribute => attribute.ProductAttributes)
            .ThenInclude(variant => variant.ProductVariants)
            .FirstOrDefaultAsync();


        if (product is null)
        {
            serviceResponse.Message = CustomConstants.NotFound.Product;
            return serviceResponse;
        }

        try
        {

            serviceResponse.Data = new ProductResponseDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                ProductMainImage = product.ProductMainImage,
                Description = product.Description,
                ProductImages = await _imageService.GetAllProductImagesPaths(product.ProductId)
            };

            if (product.ProductAttributes is not null && product.ProductAttributes.Count > 0)
            {
                serviceResponse.Data.ProductAttributesList = new List<AttributeResponseDto>();

                foreach (var attribute in product.ProductAttributes)
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

                    serviceResponse.Data.ProductAttributesList.Add(attributeResponseDto);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        serviceResponse.Message = CustomConstants.Operation.Successful;
        serviceResponse.Success = true;

        return serviceResponse;

    }

    public async Task<ServiceResponse<ProductResponseDto>> CreateProduct(CreateProductDto model, string userId)
    {
        ServiceResponse<ProductResponseDto> serviceResponse = new ServiceResponse<ProductResponseDto>();
        var newProduct = new Product();

        try
        {

            newProduct = new Product
            {
                ProductName = model.ProductName,
                Price = model.Price,
                UserId = userId,
                Description = model.Description,
                ProductAttributes = model.ProductAttributesList?.Select(attributeDto =>
                    new ProductAttribute
                    {
                        AttributeName = attributeDto.AttributeName,
                        ProductVariants = attributeDto.VariantsList?.Select(variantDto =>
                            new ProductVariant
                            {
                                VariantName = variantDto.VariantName,
                            }).ToList()

                    }).ToList()

            };

            _dbContext.Products.Add(newProduct);
            await _dbContext.SaveChangesAsync();

            if (model.Images is not null && model.Images.Length > 0)
            {

                if (model.Images.Length > 5)
                {
                    serviceResponse.Message = CustomConstants.Error.ImagesLimit;
                    return serviceResponse;
                }

                await UploadProductImages(model.Images, newProduct.ProductId);

                var productImages = await _imageService.GetAllProductImagesPaths(newProduct.ProductId);

                newProduct.ProductMainImage = productImages.FirstOrDefault();

                _dbContext.Products.Update(newProduct);
                await _dbContext.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return await GetUserProductById(newProduct.ProductId, userId);

    }

    public async Task<ServiceResponse<ProductResponseDto>> UpdateProduct(UpdateProductDto model, string userId)
    {
        ServiceResponse<ProductResponseDto> serviceResponse = new ServiceResponse<ProductResponseDto>();

        try
        {
            var product = await _dbContext.Products
                .Where(product => product.UserId.Equals(userId) && product.ProductId.Equals(model.ProductId))
                .Include(attribute => attribute.ProductAttributes)
                .ThenInclude(variants => variants.ProductVariants)
                .FirstOrDefaultAsync();

            if (product is null)
            {
                serviceResponse.Message = CustomConstants.NotFound.Product;
                return serviceResponse;
            }

            var imagesList = await _imageService.GetAllProductImagesPaths(model.ProductId);

            if (model.ProductMainImage is not null)
            {
                bool found = imagesList.Any(s => s.Contains(model.ProductMainImage));

                if (!found)
                {
                    serviceResponse.Message = CustomConstants.Error.NotExistImage;
                    return serviceResponse;
                }
            }


            product.ProductMainImage = model.ProductMainImage;
            product.ProductName = model.ProductName;
            product.Price = model.Price;
            product.Description = model.Description;

            if (model.ProductAttributesList is not null && model.ProductAttributesList.Count > 0)
            {
                foreach (var attributeDto in model.ProductAttributesList)
                {
                    var attribute = product.ProductAttributes?.FirstOrDefault(attribute => attribute.AttributeId == attributeDto.AttributeId);

                    if (attribute is not null && attribute.AttributeId > 0)
                    {
                        attribute.AttributeName = attributeDto.AttributeName;

                        if (attributeDto.VariantsList is not null && attributeDto.VariantsList.Count > 0)
                        {
                            foreach (var variantDto in attributeDto.VariantsList)
                            {
                                var variant = attribute.ProductVariants?.FirstOrDefault(variant => variant.VariantId == variantDto.VariantId);

                                if (variant is not null && variant.VariantId > 0)
                                {
                                    variant.VariantName = variantDto.VariantName;
                                    //variant.VariantImages = variantDto.VariantImages;
                                }
                                else
                                {
                                    var newVariant = new ProductVariant
                                    {
                                        VariantName = variantDto.VariantName,
                                        //VariantImages = variantDto.VariantImages
                                    };

                                    attribute.ProductVariants?.Add(newVariant);
                                }
                            }
                        }
                    }
                    else
                    {
                        var newAttribute = new ProductAttribute
                        {
                            AttributeName = attributeDto.AttributeName,
                            ProductVariants = attributeDto.VariantsList?.Select(variantDto =>
                                       new ProductVariant
                                       {
                                           VariantName = variantDto.VariantName,

                                       }).ToList()
                        };

                        product.ProductAttributes?.Add(newAttribute);
                    }
                }

            }

            if (model.Images is not null && model.Images.Length > 0)
            {
                if (imagesList.Count + model.Images.Length <= 5)
                {
                    await UploadProductImages(model.Images, product.ProductId);
                }

                serviceResponse.Message = CustomConstants.Error.ImagesLimit;
                return serviceResponse;
            }

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return await GetUserProductById(model.ProductId, userId);
    }

    public async Task<ServiceResponse<ProductResponseDto>> DeleteProduct(int productId, string userId)
    {
        ServiceResponse<ProductResponseDto> serviceResponse = new ServiceResponse<ProductResponseDto>();
        try
        {
            var product = await _dbContext.Products
                             .Where(product => product.UserId.Equals(userId) && product.ProductId.Equals(productId))
                             .FirstOrDefaultAsync();

            if (product is null)
            {
                serviceResponse.Message = CustomConstants.NotFound.Product;
                return serviceResponse;
            }

            await DeleteProductImages(productId);

            _dbContext.Products.Remove(product);

            await dbContext.SaveChangesAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

        }

        serviceResponse.Message = CustomConstants.Operation.Successful;
        serviceResponse.Success = true;
        return serviceResponse;
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

    private async Task UploadProductImages(IFormFile[] images, int newProductId)
    {

        await _imageService.Process(images.Select(i => new ImageDto
        {
            Name = i.FileName,
            Type = i.ContentType,
            Content = i.OpenReadStream()
        }), newProductId);

    }

    private async Task DeleteProductImages(int productId)
    {

        var images = await _imageService.GetAllProductImagesPaths(productId);

        if (images is not null && images.Count > 0)
        {
            foreach (var image in images)
            {
                var storagePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{image}");

                if (File.Exists(storagePath))
                {
                    File.Delete(storagePath);
                }
            }
        }
    }


}
