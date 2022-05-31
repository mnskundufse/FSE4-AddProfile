using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace Engineer.AddProfileService.Kafka
{
    public interface IProducerWrapper
    {
        Task WriteMessage(string message, string topicName);
    }

    public class ProducerWrapper: IProducerWrapper
    {
        private IProducer<string, string> _producer;
        private ProducerConfig _config;
        private static readonly Random rand = new Random();

        public ProducerWrapper(ProducerConfig config)
        {
            this._config = config;
            this._producer = new ProducerBuilder<string, string>(this._config).Build();
        }

        public async Task WriteMessage(string message, string topicName)
        {
            var dr = await _producer.ProduceAsync(topicName, new Message<string, string>()
            {
                Key = rand.Next(5).ToString(),
                Value = message
            });
            _producer.Flush(TimeSpan.FromMilliseconds(10));
            Console.WriteLine($"KAFKA => Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            return;
        }
    }
}
