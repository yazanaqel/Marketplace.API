using Marketplace.BAL.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.BAL.MediatR.Queries.ProductQueries;
public record GetAllProductsQuery(string? sortColumn, string? sortOrder, string? searchItem, int page, int pageSize)
    : IRequest<ServiceResponse<IReadOnlyList<ProductsResponseDto>>>;
