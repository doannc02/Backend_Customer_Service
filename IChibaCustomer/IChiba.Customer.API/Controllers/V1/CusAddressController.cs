using IChiba.Customer.Application.Addresses.Commands;
using IChiba.Customer.Application.Addresses.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace IChiba.Customer.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CusAddressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CusAddressController(IMediator mediator)
        {
            _mediator = mediator;
        } 
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("by-ids")]
        public async Task<IActionResult> GetListCustomersByIds([FromQuery] List<Guid> customerIds)
        {
            if (customerIds == null || !customerIds.Any())
            {
                return BadRequest("CustomerIds cannot be null or empty.");
            }

            var customers = await _mediator.Send(new  GetLstAddrByCusIdsQuery { CustomerIds = customerIds });

            if (customers == null || customers.Count == 0)
            {
                return NotFound("No address found for the given CustomerIds.");
            }

            return Ok(customers);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpPut]
        public async Task<IActionResult> SetIsDefaultAddress([FromBody]SetDefaultAddressCommand value)
        {
            if (value == null || value.CustomerId == null)
            {
                return BadRequest("CustomerId cannot be null or empty.");
            }

            var customers = await _mediator.Send(value);

            if (customers.Status == false )
            {
                return NotFound("No address found for the given customerId.");
            }

            return Ok(customers);

        }
        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
