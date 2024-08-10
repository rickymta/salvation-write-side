using Salvation.WriteSide.Models.Entities;

namespace Salvation.WriteSide.Services.MessageServices.Abstractions;

public interface IKafkaProducer
{
    Task ProduceAsync(string topic, MainTable message);
}
