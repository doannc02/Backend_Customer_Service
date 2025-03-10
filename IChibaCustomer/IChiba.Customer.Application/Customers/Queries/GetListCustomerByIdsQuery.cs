using IChiba.Customer.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IChiba.Customer.Application.Customers.Queries
{
    public record CustomerDTO
    {
        public Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public CustomerAddressDto? Address { get; set; }
    }

    public class GetListCustomerByIdsQuery : IRequest<Dictionary<Guid, CustomerDTO>>
    {
        public List<Guid> CustomerIds { get; set; } = new();
    }

    public class GetListCustomerByIdsQueryHandler : IRequestHandler<GetListCustomerByIdsQuery, Dictionary<Guid, CustomerDTO>>
    {
        private readonly CustomerDbContext _dbContext;
        private readonly ILogger<GetListCustomerByIdsQueryHandler> _logger;

        public GetListCustomerByIdsQueryHandler(CustomerDbContext dbContext, ILogger<GetListCustomerByIdsQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<Dictionary<Guid, CustomerDTO>> Handle(GetListCustomerByIdsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var customers = await _dbContext.CustomerEntities
                    .Where(c => request.CustomerIds.Contains(c.Id))
                    .Include(i => i.Addresses)
                    .AsNoTracking()  
                    .ToListAsync(cancellationToken);

                var result = new Dictionary<Guid, CustomerDTO>();

                foreach (var customer in customers)
                {
                    var defaultAddress = customer.Addresses.FirstOrDefault(a => a.IsDefaultAddress);

                    var customerDto = new CustomerDTO
                    {
                        Id = customer.Id,
                        FullName = customer.FullName,
                        Email = customer.Email,
                        PhoneNumber = customer.PhoneNumber,
                        Address = defaultAddress != null ? new CustomerAddressDto
                        {
                            Id = defaultAddress.Id,
                            City = defaultAddress.City,
                            District = defaultAddress.District,
                            Ward = defaultAddress.Ward,
                            Address = defaultAddress.Address,
                            IsDefaultAddress = defaultAddress.IsDefaultAddress,
                            Country = defaultAddress.Country,
                            Latitude = defaultAddress.Latitude,
                            Longitude = defaultAddress.Longitude
                        } : null
                    };

                    result[customer.Id] = customerDto;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customer list");
                return new Dictionary<Guid, CustomerDTO>();
            }
        }
    }
}
