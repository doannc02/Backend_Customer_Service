using IChiba.Customer.Application.Common.BaseResponse;
using IChiba.Customer.Domain.Entities;
using IChiba.Customer.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

public class AddMultipleAddressesCommand : IRequest<BaseEntity<List<Guid>>>
{
    public required Guid CustomerId { get; set; }
    public List<AddressDTO> Addresses { get; set; } = new();
}

public class AddressDTO
{
    public Guid CustomerId { get; set; }
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public Guid? CreateBy { get; set; }
    public string? Country { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

public class AddMultipleAddressesCommandHandler : IRequestHandler<AddMultipleAddressesCommand, BaseEntity<List<Guid>>>
{
    private readonly CustomerDbContext _dbContext;
    private readonly ILogger<AddMultipleAddressesCommandHandler> _logger;

    public AddMultipleAddressesCommandHandler(CustomerDbContext dbContext, ILogger<AddMultipleAddressesCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<BaseEntity<List<Guid>>> Handle(AddMultipleAddressesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await _dbContext.CustomerEntities.FindAsync(request.CustomerId);
            if (customer == null)
            {
                return new BaseEntity<List<Guid>>
                {
                    Status = false,
                    Message = "Customer not found"
                };
            }

            var newAddresses = new List<CustomerAddress>();

            foreach (var addressDto in request.Addresses)
            {
                var address = new CustomerAddress
                {
                    Id = Guid.NewGuid(),
                    CustomerId = request.CustomerId,
                    City = addressDto.City,
                    District = addressDto.District,
                    Ward = addressDto.Ward,
                    Latitude = addressDto.Latitude,
                    Longitude = addressDto.Longitude,
                    CreateAt = DateTime.UtcNow,
                    CreateBy = addressDto.CreateBy,
                    Address = addressDto.Address,
                    Country = addressDto.Country,
                };

                newAddresses.Add(address);
            }

            await _dbContext.CustomerAddresses.AddRangeAsync(newAddresses, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var addressIds = newAddresses.Select(a => a.Id).ToList();

            _logger.LogInformation("Multiple addresses added successfully.");

            return new BaseEntity<List<Guid>>
            {
                Status = true,
                Data = addressIds,
                Message = "Addresses added successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding multiple addresses");
            return new BaseEntity<List<Guid>>
            {
                Status = false,
                Message = "An error occurred while adding addresses."
            };
        }
    }
}
