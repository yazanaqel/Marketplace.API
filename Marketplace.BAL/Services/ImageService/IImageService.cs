namespace Marketplace.BAL.Services.ImageService;
public interface IImageService
{
    Task Process(IEnumerable<ImageDto> images, int productId);
    Task<List<string>> GetAllProductImagesPaths(int productId);
}