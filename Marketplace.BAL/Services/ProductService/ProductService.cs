using Marketplace.DAL.Models;
using System.Linq;

namespace Marketplace.BAL.Services.ProductService;
public class ProductService(ApplicationDbContext dbContext, IImageService imageService, IMapper mapper) : IProductService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IImageService _imageService = imageService;
    private readonly IMapper _mapper = mapper;

    public async Task<ServiceResponse<IReadOnlyList<ProductsResponseDto>>> GetAllProducts(string? sortColumn, string? sortOrder, string? searchItem, int page, int pageSize)
    {
        ServiceResponse<IReadOnlyList<ProductsResponseDto>> serviceResponse = new ServiceResponse<IReadOnlyList<ProductsResponseDto>>();

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

            var productsList = products.ConvertAll(product => new ProductsResponseDto(product));

            serviceResponse.Data = productsList.AsReadOnly();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        serviceResponse.Message = CustomConstants.Operation.Successful;
        serviceResponse.Success = true;

        return serviceResponse;

    }
    public async Task<ServiceResponse<IReadOnlyList<ProductsResponseDto>>> GetAllUserProducts(string userId, string? sortColumn, string? sortOrder, string? searchItem, int page, int pageSize)
    {
        ServiceResponse<IReadOnlyList<ProductsResponseDto>> serviceResponse = new ServiceResponse<IReadOnlyList<ProductsResponseDto>>();

        try
        {
            IQueryable<Product> productsQuery = _dbContext.Products.Where(product => product.UserId == userId);

            if (!await productsQuery.AnyAsync())
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

            var productsList = userProducts.ConvertAll(product => new ProductsResponseDto(product));

            serviceResponse.Data = productsList.AsReadOnly();

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
            IQueryable<Product> productQuery = _dbContext.Products
            .Where(product => product.ProductId == productId)
            .Include(attribute => attribute.ProductAttributes)
            .ThenInclude(variant => variant.ProductVariants);

            if (!await productQuery.AnyAsync())
            {
                serviceResponse.Message = CustomConstants.NotFound.Product;
                return serviceResponse;
            }

            var product = await productQuery.FirstOrDefaultAsync();

            serviceResponse.Data = new ProductResponseDto(product);

            serviceResponse.Data.ProductImages = await _imageService.GetAllProductImagesPaths(productId);
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

        try
        {
            IQueryable<Product> productQuery = _dbContext.Products
            .Where(product => product.ProductId == productId && product.UserId == userId)
            .Include(attribute => attribute.ProductAttributes)
            .ThenInclude(variant => variant.ProductVariants);

            if (!await productQuery.AnyAsync())
            {
                serviceResponse.Message = CustomConstants.NotFound.Product;
                return serviceResponse;
            }

            var product = await productQuery.FirstOrDefaultAsync();

            serviceResponse.Data = new ProductResponseDto(product);

            serviceResponse.Data.ProductImages = await _imageService.GetAllProductImagesPaths(productId);
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

            //newProduct = new Product
            //{
            //    ProductName = model.ProductName,
            //    Price = model.Price,
            //    UserId = userId,
            //    Description = model.Description,
            //    ProductAttributes = model.ProductAttributes?.Select(attributeDto =>
            //        new ProductAttribute
            //        {
            //            AttributeName = attributeDto.AttributeName,
            //            ProductVariants = attributeDto.ProductVariants?.Select(variantDto =>
            //                new ProductVariant
            //                {
            //                    VariantName = variantDto.VariantName,
            //                }).ToList()

            //        }).ToList()

            //};

            newProduct = _mapper.Map<Product>(model);
            newProduct.UserId = userId;

            _dbContext.Products.Add(newProduct);
            await _dbContext.SaveChangesAsync();

            if (model is { Images: not null, Images.Length: > 0 })
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
                .Where(product => product.UserId == userId && product.ProductId == model.ProductId)
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

            _mapper.Map(model, product);

            if (model.Images is not null && model.Images.Length > 0)
            {
                if (imagesList.Count + model.Images.Length <= 5)
                {
                    await UploadProductImages(model.Images, product.ProductId);
                }

                serviceResponse.Message = CustomConstants.Error.ImagesLimit;
                return serviceResponse;
            }

            _dbContext.Products.Update(product);
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
            IQueryable<Product> productQuery = _dbContext.Products
                .Where(product => product.ProductId == productId && product.UserId == userId);

            if (!await productQuery.AnyAsync())
            {
                serviceResponse.Message = CustomConstants.NotFound.Product;
                return serviceResponse;
            }

            var product = await productQuery.FirstOrDefaultAsync();

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
