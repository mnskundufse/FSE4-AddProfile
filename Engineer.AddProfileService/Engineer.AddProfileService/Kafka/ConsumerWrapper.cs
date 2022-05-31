using System;
using Confluent.Kafka;

namespace Engineer.AddProfileService.Kafka
{
    public class ConsumerWrapper: IDisposable
    {
        private string _topicName;
        private ConsumerConfig _consumerConfig;
        private IConsumer<string, string> _consumer;
        private static readonly Random rand = new Random();
        public ConsumerWrapper(ConsumerConfig config, string topicName)
        {
            this._topicName = topicName;
            this._consumerConfig = config;
            this._consumer = new ConsumerBuilder<string, string>(this._consumerConfig).Build();
            this._consumer.Subscribe(topicName);
        }
        public string ReadMessage()
        {
            try
            {
                var consumeResult = this._consumer.Consume(TimeSpan.Zero);
                return consumeResult != null && consumeResult.Message != null ? consumeResult.Message.Value : null;
            }
            catch
            {
                return null;
            }
        }

        public void Dispose()
        {
            this._consumer = null;
        }
    }
}