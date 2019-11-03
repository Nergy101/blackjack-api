using RabbitMQ.Client;
using System;
using System.Text;

namespace BlackJackAPI2.Services
{
    public class RabbitMQService: IRabbitMQService
    {
        public RabbitMQService()
        {
        }

        public void PublishVoegSpelerToeEvent(string name)
        {
            string exchangeName = "speler.events";
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

            var message = name + " joined";
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: exchangeName,
                                 routingKey: "speler.events.joined",
                                 basicProperties: null,
                                 body: body);
        }

        public void PublishSpelStartEvent()
        {
            string exchangeName = "speler.events";
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

                var message = "Started game at: " + DateTime.Now.ToString("HH:mm:ss");
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: "speler.events.game.started",
                                     basicProperties: null,
                                     body: body);
            }
        }

    }
}