using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Northwind.Application.Customers.Queries.GetCustomersList;
using Northwind.Application.Customers.Queries.GetCustomerDetail;
using Northwind.Application.Customers.Commands.UpdateCustomer;
using Northwind.Application.Customers.Commands.CreateCustomer;
using Northwind.Application.Customers.Commands.DeleteCustomer;
using MassTransit;

namespace Northwind.WebUI.Controllers
{
    public class CustomersController : BaseController
    {
        private readonly IBus _bus;

        public CustomersController(IBusControl bus)
        {
            _bus = bus;
        }

        // GET api/customers
        [HttpGet]
        public async Task<ActionResult<CustomersListViewModel>> GetAll()
        {
            return Ok(await Mediator.Send(new GetCustomersListQuery()));
        }


        public async Task<IActionResult> Send()
        {
            
            return Ok();
        }

        // GET api/customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await Mediator.Send(new GetCustomerDetailQuery { Id = id }));
        }

        // POST api/customers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateCustomerCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody]UpdateCustomerCommand command)
        {
            if (command == null || command.Id != id)
            {
                return BadRequest();
            }

            return Ok(await Mediator.Send(command));
        }

        // DELETE api/customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteCustomerCommand { Id = id });

            return NoContent();
        }
    }
}