using IChiba.Customer.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IChiba.Customer.Application.Customers.Queries;

public class GetDetailCustomerResponse
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public List<CustomerAddressDto> Addresses { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public GetDetailCustomerResponse()
    {
        Addresses = new List<CustomerAddressDto>();
    }
}

public class CustomerAddressDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string FullName { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? Address { get; set; }
    public bool IsDefaultAddress { get; set; } = false;
    public string? Country { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

public record GetDetailCustomerQuery : IRequest<GetDetailCustomerResponse>
{
    public Guid Id { get; set; }
}

public class GetDetailCustomerQueryHandler : IRequestHandler<GetDetailCustomerQuery, GetDetailCustomerResponse>
{
    private readonly IMediator _mediator;
    private readonly CustomerDbContext _dbContext;
    private readonly ILogger<GetDetailCustomerQueryHandler> _logger;

    public GetDetailCustomerQueryHandler(
        CustomerDbContext dbContext,
        IMediator mediator,
        ILogger<GetDetailCustomerQueryHandler> logger)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<GetDetailCustomerResponse> Handle(GetDetailCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.CustomerEntities
            .Include(x => x.Addresses)
            .AsNoTracking()
            .SingleOrDefaultAsync(r => r.Id == request.Id);

        if (customer == null) return null;
        var todo = new GetDetailCustomerResponse()
        {
            Id = customer.Id,
            PhoneNumber = customer.PhoneNumber,
            Addresses = customer.Addresses.Select(x => new CustomerAddressDto
            {
                Id = x.Id,
                City = x.City,
                District = x.District,
                Ward = x.Ward,
                CustomerId = x.CustomerId,
                IsDefaultAddress = x.IsDefaultAddress,
                Address = x.Address
            }).ToList(),
            Email = customer.Email,
            FullName = customer.FullName,
            CreatedAt = customer.CreatedAt,
        };

        return todo;
    }
}
