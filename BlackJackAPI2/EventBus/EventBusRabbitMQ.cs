using BlackJackAPI2.EventBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackAPI2.Eventbus
{
    public class EventBusRabbitMQ : IDisposable
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private IModel _consumerChannel;
        private string _queueName;
        private readonly IEventService _eventService;
        //UserService _userService = new UserService();

        public EventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection, IEventService eventService, string queueName = null)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _queueName = queueName;
            _eventService = eventService;
        }

        public IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();

            if (_queueName == null)
            {
                _queueName = channel.QueueDeclare().QueueName;
            }
            else
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: _queueName,
                  exchange: "api.events",
                  routingKey: "api.events.*");
            }
            var consumer = new EventingBasicConsumer(channel);

            //consumer.Received += async (model, e) =>
            //{
            //    var eventName = e.RoutingKey;
            //    var message = Encoding.UTF8.GetString(e.Body);
            //    channel.BasicAck(e.DeliveryTag, multiple: false);
            //};

            //Create event when something receive
            consumer.Received += ReceivedEvent;



            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };
            return channel;
        }

        private void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == "api.events.removenumber")
            {
                //Implementation here
            }

            if (e.RoutingKey == "api.events.addnumber")
            {
                var numberToAdd=int.Parse(Encoding.UTF8.GetString(e.Body));
                _eventService.AddNumber(numberToAdd);
            }
        }

        public void PublishUserSaveFeedback(string _queueName/*,*//* UserSaveFeedback publishModel, *//*IDictionary<string, object> headers*/)
        {

            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using var channel = _persistentConnection.CreateModel();
            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            //var message = JsonConvert.SerializeObject(publishModel);
            var body = Encoding.UTF8.GetBytes("message"/*message*/);

            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 1;
            //properties.Headers = headers;
            properties.Expiration = "3600";
            //properties.ContentType = "application/json";

            channel.ConfirmSelect();
            channel.BasicPublish(exchange: "", routingKey: _queueName, mandatory: true, basicProperties: properties, body: body);
            channel.WaitForConfirmsOrDie(new TimeSpan(3000));

            channel.BasicAcks += (sender, eventArgs) =>
            {
                Console.WriteLine("Sent RabbitMQ");
                //implement ack handle
            };
            channel.ConfirmSelect();
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
        }
    }
}