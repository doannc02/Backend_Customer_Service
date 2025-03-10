using IChiba.Customer.Application.Common.BaseResponse;
using IChiba.Customer.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IChiba.Customer.Application.Addresses.Commands
{
    public class SetDefaultAddressCommand : IRequest<BaseEntity<bool>>
    {
        public Guid CustomerId { get; set; } 
        public Guid NewDefaultAddressId { get; set; }
    }

    public class SetDefaultAddressCommandHandler : IRequestHandler<SetDefaultAddressCommand, BaseEntity<bool>>
    {
        private readonly CustomerDbContext _dbContext;
        private readonly ILogger<SetDefaultAddressCommandHandler> _logger;

        public SetDefaultAddressCommandHandler(CustomerDbContext dbContext, ILogger<SetDefaultAddressCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<BaseEntity<bool>> Handle(SetDefaultAddressCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customerAddresses = await _dbContext.CustomerAddresses
                    .Where(c => c.CustomerId == request.CustomerId)
                    .ToListAsync(cancellationToken);

                if (!customerAddresses.Any())
                {
                    return new BaseEntity<bool>
                    {
                        Status = false,
                        Message = "Customer does not have any addresses.",
                        Data = false
                    };
                }

                var newDefaultAddress = customerAddresses.FirstOrDefault(a => a.Id == request.NewDefaultAddressId);

                if (newDefaultAddress == null)
                {
                    return new BaseEntity<bool>
                    {
                        Status = false,
                        Message = "The specified address does not exist.",
                        Data = false
                    };
                }

                foreach (var address in customerAddresses)
                {
                    address.IsDefaultAddress = false;
                }

                newDefaultAddress.IsDefaultAddress = true;

                _dbContext.CustomerAddresses.UpdateRange(customerAddresses);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new BaseEntity<bool>
                {
                    Status = true,
                    Message = "Default address updated successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating default address.");
                return new BaseEntity<bool>
                {
                    Status = false,
                    Message = "An error occurred while updating the default address.",
                    Data = false
                };
            }
        }
    }
}
