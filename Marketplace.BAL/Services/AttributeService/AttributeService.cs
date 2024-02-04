using Marketplace.BAL.DbContext;
using Marketplace.BAL.Services.ProductService;
using Marketplace.DAL.Dtos.ProductDtos;
using Marketplace.DAL.Models;
using Marketplace.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketplace.DAL.Dtos.AttributeDto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Marketplace.BAL.Services.AttributeService;
public class AttributeService(ApplicationDbContext dbContext, IProductService productService) : IAttributeService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IProductService _productService = productService;


    public async Task<ServiceResponse<ProductResponseDto>> AddAttributes(AddAttributeDto model, string userId)
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

            if (model.AttributesList.Count > 0)
            {
                var newAttributes = new List<DAL.Models.Attribute>();
                foreach (var attribute in model.AttributesList)
                {
                    newAttributes.Add(
                        new DAL.Models.Attribute
                        {
                            AttributeName = attribute,
                            ProductId = product.ProductId
                        });
                }

                await _dbContext.Attributes.AddRangeAsync(newAttributes);
                await _dbContext.SaveChangesAsync();
            }


        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        return await _productService.GetUserProductById(model.ProductId, userId);

    }

    public async Task<ServiceResponse<ProductResponseDto>> UpdateAttribute(UpdateAttributeDto model, string userId)
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

            attribute.AttributeName = model.AttributeName;
            attribute.AttributeId = model.AttributeId;

            _dbContext.Attributes.Update(attribute);
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


    public async Task<ServiceResponse<List<ProductResponseDto>>> DeleteAttribute(int attributeId, string userId)
    {
        ServiceResponse<List<ProductResponseDto>> serviceResponse = new ServiceResponse<List<ProductResponseDto>>();

        try
        {

            var attribute = await _dbContext.Attributes
                .Include(attribute => attribute.Product)
                .FirstOrDefaultAsync(attribute => attribute.AttributeId.Equals(attributeId));

            if (attribute is null || !attribute.Product.UserId.Equals(userId))
            {
                serviceResponse.Message = $"Attribute With Id: {attributeId} Not Found";
                return serviceResponse;
            }

            _dbContext.Attributes.Remove(attribute);

            await dbContext.SaveChangesAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        return await _productService.GetAllUserProducts(userId, null, null,null, 1, 5);
    }


}
