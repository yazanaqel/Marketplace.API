global using Microsoft.EntityFrameworkCore;
global using Marketplace.BAL.DbContext;
global using Marketplace.DAL.Dtos.ProductDtos;
global using Marketplace.BAL.Services.ProductService;
global using Marketplace.DAL.Models;
global using Microsoft.AspNetCore.Http;
global using Marketplace.BAL.Services.ImageService;
global using Marketplace.DAL.Dtos.AttributeDto;
global using Marketplace.DAL.Dtos.ProductVariantDto;
global using System.Linq.Expressions;
global using Marketplace.DAL.Dtos;
global using Marketplace.BAL.Constants;

namespace Marketplace.BAL.Services;
public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
