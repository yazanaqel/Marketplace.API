using Azure;
using Marketplace.BAL.DbContext;
using Marketplace.BAL.Services.ProductService;
using Marketplace.DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.BAL.Services.ImageService;
public class ImageService(IServiceScopeFactory serviceScopeFactory, ApplicationDbContext dbContext) : IImageService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private const int thumbnailWidth = 300;


    public async Task<List<string>> GetAllProductImagesIds(int productId)
    {
        return await _dbContext
            .ProductImages.Where(id => id.ProductId.Equals(productId))
            .Select(x => x.ImageId.ToString())
            .ToListAsync();
    }

    public async Task Process(IEnumerable<ImageInputModel> images, int productId)
    {

        var tasks = images
            .Select(image => Task.Run(async () =>
            {
                try
                {
                    using var imageResult = await Image.LoadAsync(image.Content);

                    var original = await SaveImage(imageResult, imageResult.Width);
                    var thumbnail = await SaveImage(imageResult, thumbnailWidth);

                    var data = _serviceScopeFactory
                    .CreateScope()
                    .ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                    data.ProductImages.Add(new ProductImages
                    {
                        OriginalFileName = image.Name,
                        OriginalType = image.Type,
                        OriginalContent = original,
                        ThumbnailContent = thumbnail,
                        ProductId = productId
                    });

                    await data.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }))
            .ToList();

        await Task.WhenAll(tasks);
    }

    public Task<Stream> GetThumbnail(string id)
        => GetImageData(id, "Thumbnail");

    private async Task<byte[]> SaveImage(Image image, int resizeWidth)
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

        var memoryStream = new MemoryStream();

        await image.SaveAsJpegAsync(memoryStream, new JpegEncoder
        {
            Quality = 75
        });

        return memoryStream.ToArray();

    }

    private async Task<Stream> GetImageData(string id, string size)
    {
        var database = _dbContext.Database;

        var dbconnection = (SqlConnection)database.GetDbConnection();

        var command = new SqlCommand(
            $"SELECT {size}Content FROM ProductImages WHERE Id=@id"
            , dbconnection);

        command.Parameters.Add(new SqlParameter("@id", id));
        dbconnection.Open();
        var reader = await command.ExecuteReaderAsync();
        Stream result = null;
        if (reader.HasRows)
        {
            while (reader.Read()) result = reader.GetStream(0);
        }

        reader.Close();

        return result;
    }
}
