using IChiba.Customer.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IChiba.Customer.Application.Addresses.Queries
{
    public class GetLstAddrByCusIdsQueryResponse
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public CustomerDto Customer { get; set; }
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public Guid? CreateBy { get; set; }
        public string? Country { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public record CustomerDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class GetLstAddrByCusIdsQuery : IRequest<Dictionary<Guid, GetLstAddrByCusIdsQueryResponse>>
    {
        public required List<Guid> CustomerIds { get; set; }
    }

    public class GetLstAddrByCusIdsQueryHandler : IRequestHandler<GetLstAddrByCusIdsQuery, Dictionary<Guid, GetLstAddrByCusIdsQueryResponse>>
    {
        private readonly CustomerDbContext _dbContext;
        private readonly ILogger<GetLstAddrByCusIdsQueryHandler> _logger;

        public GetLstAddrByCusIdsQueryHandler(CustomerDbContext dbContext, ILogger<GetLstAddrByCusIdsQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Dictionary<Guid, GetLstAddrByCusIdsQueryResponse>> Handle(GetLstAddrByCusIdsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Lấy danh sách các địa chỉ của khách hàng, chỉ lấy địa chỉ mặc định
                var addresses = await _dbContext.CustomerAddresses
                    .Where(c => query.CustomerIds.Contains(c.CustomerId) && c.IsDefaultAddress == true) 
                    .Include(i => i.Customer)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                // hashmap<customerId, adddrss>
                return addresses
                    .ToDictionary(
                        a => a.CustomerId, 
                        a => new GetLstAddrByCusIdsQueryResponse
                        {
                            Id = a.Id,
                            District = a.District,
                            Address = a.Address,
                            Ward = a.Ward,
                            Customer = new CustomerDto
                            {
                                Id = a.CustomerId,
                                Email = a.Customer.Email,
                                FullName = a.Customer.FullName,
                                PhoneNumber = a.Customer.PhoneNumber
                            },
                            City = a.City,
                            Country = a.Country,
                            CreateBy = a.CreateBy,
                            Longitude = a.Longitude,
                            Latitude = a.Latitude
                        }
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customer list");
                return new Dictionary<Guid, GetLstAddrByCusIdsQueryResponse>(); 
            }
        }
    }
}
