using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N5PermissionsAPI.Infrastructure.Configuration;
using System.Text.Json;

namespace N5PermissionsAPI.Infrastructure.Services;

public class KafkaService
{
    private readonly ProducerConfig _producerConfig;
    private readonly ConsumerConfig _consumerConfig;
    private readonly string _topic;
    private readonly ILogger<KafkaService> _logger;

    public KafkaService(IOptions<KafkaOptions> kafkaOptions, ILogger<KafkaService> logger)
    {
        var options = kafkaOptions.Value;
        _producerConfig = new ProducerConfig
        {
            BootstrapServers = options.BootstrapServers,
            ClientId = options.ClientId
        };
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = options.BootstrapServers,
            GroupId = options.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _topic = options.Topic;
        _logger = logger;
    }

    public async Task ProduceMessageAsync(string operation, object data)
    {
        if (string.IsNullOrWhiteSpace(operation))
        {
            throw new ArgumentException("Operation name cannot be null or empty", nameof(operation));
        }

        try
        {
            using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
            var message = new KafkaOperationDto
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                OperationName = operation,
                Data = data
            };

            var serializedMessage = JsonSerializer.Serialize(message);
            await producer.ProduceAsync(_topic, new Message<Null, string> { Value = serializedMessage });
            _logger.LogInformation("Message produced to Kafka successfully. Operation: {Operation}", operation);
        }
        catch (ProduceException<Null, string> ex)
        {
            _logger.LogError(ex, "Failed to produce message to Kafka. Operation: {Operation}", operation);
        }
    }
}

public class KafkaOperationDto
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string OperationName { get; set; }
    public object Data { get; set; }
}