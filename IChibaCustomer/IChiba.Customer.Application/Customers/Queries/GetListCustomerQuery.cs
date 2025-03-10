using IChiba.Customer.Application.Common.BaseRequest;
using IChiba.Customer.Application.Common.BaseResponse;
using IChiba.Customer.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
namespace IChiba.Customer.Application.Customers.Queries;

public class GetListCustomerQueryResposone
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public List<CustomerAddressDto> Addresses { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
public class GetListCustomerQuery : QueryPage, IRequest<PageResponse<GetListCustomerQueryResposone>> { }

public class GetListCustomerQueryHandler : IRequestHandler<GetListCustomerQuery, PageResponse<GetListCustomerQueryResposone>>
{
    private readonly CustomerDbContext _dbContext;
    private readonly ILogger<GetListCustomerQueryHandler> _logger;
    public GetListCustomerQueryHandler(CustomerDbContext dbContext, ILogger<GetListCustomerQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PageResponse<GetListCustomerQueryResposone>> Handle(GetListCustomerQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var queryable = _dbContext.CustomerEntities
                .AsNoTracking()
                .OrderBy(c => c.CreatedAt)
                .Include(i => i.Addresses) // Đảm bảo rằng bạn vẫn Include Addresses
                .Select(c => new GetListCustomerQueryResposone
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    Addresses = c.Addresses.Select(addr => new CustomerAddressDto
                    {
                        Id = addr.Id,  
                        City = addr.City,
                        District = addr.District,
                        Ward = addr.Ward,
                        Address = addr.Address,
                        Latitude = addr.Latitude,
                        Longitude = addr.Longitude,
                        Country = addr.Country,
                        CustomerId = c.Id,
                    }).ToList()
                });

            var totalElements = await queryable.CountAsync(cancellationToken);

            var customers = await queryable
                .Skip((query.Page - 1) * query.Size)
                .Take(query.Size)
                .ToListAsync(cancellationToken);

            return new PageResponse<GetListCustomerQueryResposone>
            {
                Status = true,
                Message = "Get list customer Success",
                Data = new Data<GetListCustomerQueryResposone>
                {
                    Content = customers,  
                    Page = query.Page,
                    Size = query.Size,
                    TotalElements = totalElements,
                    TotalPages = (int)Math.Ceiling(totalElements / (double)query.Size),
                    NumberOfElements = customers.Count
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching customer list.");
            return new PageResponse<GetListCustomerQueryResposone>
            {
                Status = false,
                Message = "An error occurred while fetching customer list.",
                Data = new Data<GetListCustomerQueryResposone>
                {
                    Content = new List<GetListCustomerQueryResposone>(),
                    Page = query.Page,
                    Size = query.Size,
                    TotalElements = 0,
                    TotalPages = 0,
                    NumberOfElements = 0
                }
            };
        }
    }

}

