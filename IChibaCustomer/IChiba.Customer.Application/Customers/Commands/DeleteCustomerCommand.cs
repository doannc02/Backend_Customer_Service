using IChiba.Customer.Application.Common.BaseResponse;
using IChiba.Customer.Domain.Entities;
using IChiba.Customer.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IChiba.Customer.Application.Customers.Commands
{
    public class DeleteCustomerCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class DeleteCustomerCommand : IRequest<BaseEntity<DeleteCustomerCommandResponse>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, BaseEntity<DeleteCustomerCommandResponse>>
    {
        private readonly CustomerDbContext _dbContext;
        private readonly ILogger<DeleteCustomerCommandHandler> _logger;
        public DeleteCustomerCommandHandler(CustomerDbContext dbContext, ILogger<DeleteCustomerCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<BaseEntity<DeleteCustomerCommandResponse>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerEntity = await _dbContext.CustomerEntities
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (customerEntity == null)
            {
                _logger.LogInformation($"Customer not found.");

                return new BaseEntity<DeleteCustomerCommandResponse>
                { Status = false, Message = "Customer not found.", Data = null };

            }

            _dbContext.CustomerEntities.Remove(customerEntity);

            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Deleted successfully.");

            return new BaseEntity<DeleteCustomerCommandResponse>
            {
                Status = true,
                Message = "Customer deleted successfully.",
                Data = new DeleteCustomerCommandResponse { Id = request.Id }
            };
        }
    }
}
