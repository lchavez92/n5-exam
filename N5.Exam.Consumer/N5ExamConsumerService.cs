using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N5.Exam.Domain.Models;
using N5.Exam.Domain.Options;

namespace N5.Exam.Consumer
{
    public class N5ExamConsumerService : IHostedService
    {
        private readonly IOptions<KafkaSettings> _settings;
        private readonly ILogger<N5ExamConsumerService> _logger;

        public N5ExamConsumerService(IOptions<KafkaSettings> settings, ILogger<N5ExamConsumerService> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _settings.Value.GroupId,
                BootstrapServers = _settings.Value.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            try
            {
                using var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build();
                consumerBuilder.Subscribe(_settings.Value.Topic);
                var cancelToken = new CancellationTokenSource();

                try
                {
                    while (true)
                    {
                        var consumer = consumerBuilder.Consume(cancelToken.Token);
                        var request = JsonSerializer.Deserialize
                            <PermissionRequest>
                            (consumer.Message.Value);

                        if (request == null)
                        {
                            continue;
                        }

                        _logger.LogInformation($"Processing Request Id:{request.Id}, Operation Name: {request.OperationName}");
                    }
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
