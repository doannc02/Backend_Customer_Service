using IChiba.Customer.Application.Customers.Commands;
using IChiba.Customer.Application.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IChiba.Customer.API.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }
    // GET: api/<CustomerController>
    [HttpGet("/list")]
    public async Task<IActionResult> GetList([FromQuery]GetListCustomerQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // GET api/<CustomerController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById( Guid id)
    {
        if (id == Guid.Empty) return BadRequest("ID must not be empty.");

        var request = new GetDetailCustomerQuery()
        {
            Id = id
        };

        var result = await _mediator.Send(request);
        return Ok(result);
    }


    // GET api/<CustomerController>/5
    [HttpGet("by-ids")]
    public async Task<IActionResult> GetListCustomersByIds([FromQuery] List<Guid> customerIds)
    {
        if (customerIds == null || !customerIds.Any())
        {
            return BadRequest("CustomerIds cannot be null or empty.");
        }

        var customers = await _mediator.Send(new GetListCustomerByIdsQuery { CustomerIds = customerIds });

        if (customers == null || customers.Count == 0)
        {
            return NotFound("No customers found for the given IDs.");
        }

        return Ok(customers);
    }


    // POST api/<CustomerController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]CreateCustomerCommand value)
    {
        if (value == null) return BadRequest("value must not be empty.");

        var result = await _mediator.Send(value);
        return Ok(result);
    }

    // PUT api/<CustomerController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody]UpdateCustomerCommand value)
    {
        if (id == Guid.Empty) return BadRequest("ID must not be empty.");

        var result = await _mediator.Send(value);
        return Ok(result);
    }

    // DELETE api/<CustomerController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty) return BadRequest("ID must not be empty.");

        var request = new DeleteCustomerCommand()
        {
            Id = id
        };

        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
