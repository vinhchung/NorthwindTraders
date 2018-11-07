using MassTransit;
using Northwind.Application.Messages;
using System;

namespace Northwind
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
                var host = sbc.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                sbc.ReceiveEndpoint(host, "test_queue", ep =>
                    {
                        ep.Handler<IMessage>(context =>
                        {
                            return Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
                        });
                    });
             });
            bus.Start();
            Console.ReadLine();
        }
    }
}
