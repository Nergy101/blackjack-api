using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using RabbitMQClient.Model;

namespace RabbitMQClient
{
    class Program
    {
        static void Main(string[] args)
        {
            bool loop = true;
            while (loop)
            {
                Console.WriteLine("Voer uw naam in:");
                var input = Console.ReadLine();
                if (input == "#start-game")
                {
                    new SpelerAgent().StartGame();
                    Console.WriteLine("Starting Game");
                }
                else if (input == "#addnumber")
                {
                    Console.WriteLine("Getal om te sturen:");
                    var getal = Console.ReadLine();
                    string exchangeNameForEvent = "api.events";
                    var factoryForEvent = new ConnectionFactory() { HostName = "localhost" };
                    using var connection = factoryForEvent.CreateConnection();
                    using var channel = connection.CreateModel();
                    channel.ExchangeDeclare(exchange: exchangeNameForEvent, type: ExchangeType.Topic);

                    var message = getal.ToString();
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: exchangeNameForEvent,
                                            routingKey: "api.events.addnumber",
                                            basicProperties: null,
                                            body: body);

                }
                else
                {
                    var speler = new ToegevoegdeSpeler(input);
                    new SpelerAgent().AddSpeler(speler);
                }

                // RabbitMQ Receive
                string exchangeName = "speler.events";

                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
                    //channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

                    var queueName = channel.QueueDeclare().QueueName; // random queuename 
                    channel.QueueBind(queue: queueName,
                                      exchange: exchangeName,
                                      routingKey: "speler.events.*");
                    channel.QueueBind(queue: queueName,
                                      exchange: exchangeName,
                                      routingKey: "speler.events.game.*");

                    Console.WriteLine(" [*] Waiting for logs.");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($" [x] {message}");
                    };
                    channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);

                    Console.WriteLine(" Press [enter] to enter another name.");
                    Console.ReadLine();
                }
            }
        }
    }
}
