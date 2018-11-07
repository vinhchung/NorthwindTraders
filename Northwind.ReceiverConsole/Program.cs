using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

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
                        queue: "task_queue",
                        durable: true,
                        exclusive:false,
                        autoDelete:false,
                        arguments: null
                        );

                    channel.BasicQos(0, 1, false);
                    Console.WriteLine("Waiting for messages...");
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, e) =>
                    {
                        var body = e.Body;
                        
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"Message received: {message}");

                        int dots = message.Split('.').Length - 1;
                        Task.Delay(dots * 1000).Wait();
                        Console.WriteLine("Consumer processing done: send ack");
                        channel.BasicAck(e.DeliveryTag, false);
                    };
                    channel.BasicConsume("task_queue", false, consumer);
                    Console.Write("Press enter to exit");
                    Console.ReadLine();
                }
            }
        }
    }
}
