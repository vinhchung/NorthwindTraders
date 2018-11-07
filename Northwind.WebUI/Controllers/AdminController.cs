using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Application.Employees.Commands;
using Northwind.Application.Employees.Models;
using Northwind.Application.Employees.Queries;
using RabbitMQ.Client;

namespace Northwind.WebUI.Controllers
{
    public class AdminController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Hello()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);

                    await Task.Run(() => channel.BasicPublish(exchange: "",
                                         routingKey: "hello",
                                         basicProperties: null,
                                         body: body));

                    return Ok($"[x] Sent {message}");
                }
            }
        }

        [HttpGet]
        public Task<IEnumerable<EmployeeManagerModel>> EmployeeManagerReport()
        {
            return Mediator.Send(new EmployeesWithManagersQuery());
        }

        [HttpPost]
        public IActionResult ChangeEmployeeManager(ChangeEmployeesManagerCommand command)
        {
            Mediator.Send(command);

            return NoContent();
        }
    }
}
