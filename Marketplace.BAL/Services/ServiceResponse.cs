global using Microsoft.EntityFrameworkCore;
global using Marketplace.BAL.DbContext;
global using Marketplace.BAL.Dtos.ProductDtos;
global using Marketplace.BAL.Services.ProductService;
global using Marketplace.DAL.Models;
global using Microsoft.AspNetCore.Http;
global using Marketplace.BAL.Services.ImageService;
global using Marketplace.BAL.Dtos.AttributeDto;
global using Marketplace.BAL.Dtos.ProductVariantDto;
global using System.Linq.Expressions;
global using Marketplace.BAL.Dtos;
global using Marketplace.BAL.Constants;
global using AutoMapper;
namespace Marketplace.BAL.Services;
public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public ErrorDetails? Error { get; set; }
    public PaginationMetadata? Pagination { get; set; }
}
public class ErrorDetails
{
    public string ErrorCode { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}
public class PaginationMetadata
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}