using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Northwind.ReceiverConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "hello",
                        durable: false,
                        exclusive:false,
                        autoDelete:false,
                        arguments: null
                        );
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, e) =>
                    {
                        var body = e.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"Message received: {message}");
                    };
                    channel.BasicConsume("hello", false, consumer);
                    Console.Write("Press enter to exit");
                    Console.ReadLine();
                }
            }
        }
    }
}
