using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Marketplace.BAL.Services.ImageService;
public class ImageService(IServiceScopeFactory serviceScopeFactory, ApplicationDbContext dbContext) : IImageService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private const int thumbnailWidth = 300;


    public Task<List<string>> GetAllProductImagesPaths(int productId)
    {
        IQueryable<ProductImages> paths = _dbContext
            .ProductImages
            .Where(x => x.ProductId == productId);

        if (paths.Any())
        {
            return paths.Select(x => x.Folder + "Thumbnail_" + x.Id + ".jpg").ToListAsync();
        }
        else
        {
            return Task.FromResult(new List<string>());
        }
    }

    public async Task Process(IEnumerable<ImageDto> images, int productId)
    {

        var totalImages = await _dbContext.ProductImages.CountAsync();

        var tasks = images
            .Select(image => Task.Run(async () =>
            {
                try
                {
                    using var imageResult = await Image.LoadAsync(image.Content);

                    var id = Guid.NewGuid();
                    var path = $"/images/{totalImages % 1000}/";
                    var name = $"{id}.jpg";
                    var storagePath = Path.Combine(
                        Directory.GetCurrentDirectory(), $"wwwroot{path}".Replace("/", "\\"));

                    if (!Directory.Exists(storagePath))
                    {
                        Directory.CreateDirectory(storagePath);
                    }

                    await SaveImage(imageResult, $"Thumbnail_{name}", storagePath, thumbnailWidth);

                    var data = _serviceScopeFactory
                        .CreateScope()
                        .ServiceProvider
                        .GetRequiredService<ApplicationDbContext>();

                    data.ProductImages.Add(new ProductImages
                    {
                        Id = id,
                        Folder = path,
                        ProductId = productId,
                    });

                    await data.SaveChangesAsync();
                }
                catch
                {
                    //loger info
                }

            }))
            .ToList();



        await Task.WhenAll(tasks);
    }
    private async Task SaveImage(Image image, string name, string path, int resizeWidth)
    {
        var width = image.Width;
        var height = image.Height;

        if (width > resizeWidth)
        {
            height = (int)((double)resizeWidth / width * height);
            width = resizeWidth;
        }

        image.Mutate(i => i.Resize(new Size(width, height)));

        image.Metadata.ExifProfile = null;

        await image.SaveAsJpegAsync($"{path}/{name}", new JpegEncoder
        {
            Quality = 75
        });

    }
}
