using IChiba.Customer.Domain.Entities;
using IChiba.Customer.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using IChiba.Customer.Application.Common.BaseResponse;

namespace IChiba.Customer.Application.Customers.Commands;

public class UpdateCustomerCommandResponse
{
    public required Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual ICollection<CustomerAddressDto> Addresses { get; set; } = new List<CustomerAddressDto>();
}

public class UpdateCustomerCommand : IRequest<BaseEntity<UpdateCustomerCommandResponse>>
{
    public required Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public virtual ICollection<CustomerAddressDto> Addresses { get; set; } = new List<CustomerAddressDto>();
}

public class CustomerAddressDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string Ward { get; set; }
    public string Address { get; set; }
}

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, BaseEntity<UpdateCustomerCommandResponse>>
{
    private readonly CustomerDbContext _dbContext;
    private readonly ILogger<UpdateCustomerCommandHandler> _logger;

    public UpdateCustomerCommandHandler(CustomerDbContext dbContext, ILogger<UpdateCustomerCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<BaseEntity<UpdateCustomerCommandResponse>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var resultUpdated = new BaseEntity<UpdateCustomerCommandResponse>();
        var customerEntity = await _dbContext.CustomerEntities
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (customerEntity == null)
        {
            _logger.LogWarning($"Customer with ID {request.Id} not found.");

            resultUpdated.Data = null;
            resultUpdated.Status = false;
            return resultUpdated;
            // throw new KeyNotFoundException($"Customer with ID {request.Id} not found.");
        }

        customerEntity.FullName = request.FullName;
        customerEntity.Email = request.Email;
        customerEntity.PhoneNumber = request.PhoneNumber;
        customerEntity.UpdatedAt = DateTime.UtcNow;

        var existingAddresses = customerEntity.Addresses.ToList();

        foreach (var address in request.Addresses)
        {
            var existingAddress = existingAddresses.FirstOrDefault(a => a.Id == address.Id);

            if (existingAddress != null)
            {
                existingAddress.Address = address.Address;
                existingAddress.City = address.City;
                existingAddress.District = address.District;
                existingAddress.Ward = address.Ward;
            }
            else
            {
                customerEntity.Addresses.Add(new CustomerAddress
                {
                    Id = Guid.NewGuid(), 
                    CustomerId = request.Id,
                    Address = address.Address,
                    City = address.City,
                    District = address.District,
                    Ward = address.Ward
                });
            }
        }

        foreach (var address in existingAddresses)
        {
            if (!request.Addresses.Any(a => a.Id == address.Id))
            {
                _dbContext.CustomerAddresses.Remove(address);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        var customerDTOUpdated = new UpdateCustomerCommandResponse
        {
            Id = customerEntity.Id,
            FullName = customerEntity.FullName,
            Email = customerEntity.Email,
            PhoneNumber = customerEntity.PhoneNumber,
            CreatedAt = customerEntity.CreatedAt,
            UpdatedAt = customerEntity.UpdatedAt,
            Addresses = customerEntity.Addresses.Select(a => new CustomerAddressDto
            {
                Id = a.Id,
                CustomerId = a.CustomerId,
                Address = a.Address,
                City = a.City,
                District = a.District,
                Ward = a.Ward
            }).ToList()
        };
        resultUpdated.Data = customerDTOUpdated;
        resultUpdated.Status = true;
        return resultUpdated;
    }
}

