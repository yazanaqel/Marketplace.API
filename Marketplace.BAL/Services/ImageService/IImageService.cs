using Marketplace.DAL.Models;

namespace Marketplace.BAL.Services.ImageService;
public interface IImageService
{
    Task<List<string>> GetAllProductImagesIds(int productId);
    Task<Stream> GetThumbnail(string id);
    Task Process(IEnumerable<ImageInputModel> images, int productId);
}