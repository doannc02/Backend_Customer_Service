using IChiba.Customer.Domain.Entities;
using IChiba.Customer.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;


namespace IChiba.Customer.Application.Customers.Commands;

public class CreateCustomerCommandResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();
}

public class CustomerAddressDTO
{
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Country { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

public class CreateCustomerCommand : IRequest<CreateCustomerCommandResponse>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Guid CreateBy { get; set; }
    public List<CustomerAddressDTO>? Addresses { get; set; } = new();
}


public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerCommandResponse>
{
    private readonly CustomerDbContext _dbContext;
    private readonly ILogger<CreateCustomerCommandHandler> _logger;

    public CreateCustomerCommandHandler(
        CustomerDbContext dbContext,
        ILogger<CreateCustomerCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var idCustomer = Guid.NewGuid();
        var customer = new CustomerEntity()
        {
            Id = idCustomer,
            Email = request.Email,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            Addresses = request.Addresses?.Select(addr => new CustomerAddress
            {
                Id = Guid.NewGuid(), 
                City = addr.City,
                District = addr.District,
                Ward = addr.Ward,
                Address = addr.Address,
                Latitude = addr.Latitude,
                Longitude = addr.Longitude,
                Country  = addr.Country,
                CreateBy = request.CreateBy,
                CustomerId = idCustomer,
                CreateAt = DateTime.UtcNow,
                
            }).ToList() ?? new List<CustomerAddress>()
        };

        _dbContext.CustomerEntities.Add(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"Customer {customer} created successfully.");
        return new CreateCustomerCommandResponse
        {
            Id = customer.Id,
        };
    }


}
