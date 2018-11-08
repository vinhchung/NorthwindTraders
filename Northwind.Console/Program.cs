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
                            return Console.Out.WriteLineAsync($"Message ep 1 Received: {context.Message.Text}");
                        });
                    });

                sbc.ReceiveEndpoint(host, "test_queue2", ep =>
                {
                    ep.Handler<IMessage>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Message ep 2 Received: {context.Message.Text}");
                    });

                    ep.Handler<IFile>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Fle ep 2 Received: {context.Message.Name}");
                    });
                });
            });
            bus.Start();
            Console.ReadLine();
        }
    }
}
