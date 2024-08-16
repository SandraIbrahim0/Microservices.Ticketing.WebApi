using MessageConsumer.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MessageConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IModel _channel;
        private string exchangeName = "EmailExchange";
        private string routingKey = "email_queue";
        private string queueName = "EmailQueue";
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            InitRabbitMQ();

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    StartConsuming(queueName, stoppingToken);
                }
            }

        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672/")
            };

            factory.ClientProvidedName = "Rabbit Test Consumer";

            // create connection
            _connection = factory.CreateConnection();

            // create channel
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: queueName, exchangeName, routingKey, null);
            _channel.BasicQos(0, 1, false);
        }
        private void StartConsuming(string queueName, CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                bool processedSuccessfully = false;
                try
                {
                    Console.WriteLine($"Message received: {message}");
                    EmailService emailService = new EmailService();
                    emailService.SendGMail(message);
                    processedSuccessfully = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while processing message from queue {queueName}: {ex}");
                }

                if (processedSuccessfully)
                {
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                else
                {
                    _channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            Console.WriteLine("Waiting for messages. Press Q to quit.");

            // Sleep for a long time to keep the application running
            Console.WriteLine("Press Any Key to Continue..");
            Console.ReadLine();
            // Close resources before exiting
            _channel.Close();
            _connection.Close();
        }
    }
}
