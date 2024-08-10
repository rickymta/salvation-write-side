using Confluent.Kafka;
using Newtonsoft.Json;
using Salvation.WriteSide.Commons;
using Salvation.WriteSide.Models.Entities;
using Salvation.WriteSide.Services.MessageServices.Abstractions;

namespace Salvation.WriteSide.Services.MessageServices.Implementations;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(IConfiguration configuration)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task ProduceAsync(string topic, MainTable message)
    {
        try
        {
            var jsonMessage = JsonConvert.SerializeObject(message);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });
        }
        catch (Exception ex)
        {
            LogProvider.Error("KafkaProducer", ex);
        }
    }
}
