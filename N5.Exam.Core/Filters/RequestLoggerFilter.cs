using System;
using System.Net;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N5.Exam.Domain.Models;
using N5.Exam.Domain.Options;
using Newtonsoft.Json;

namespace N5.Exam.Domain.Filters
{
    public class RequestLoggerFilter : IAsyncActionFilter
    {

        private readonly ILogger<RequestLoggerFilter> _logger;
        private readonly IOptions<KafkaSettings> _settings;

        public RequestLoggerFilter(ILogger<RequestLoggerFilter> logger, IOptions<KafkaSettings> settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            _logger.LogInformation(
                $"Request:{context.HttpContext.Request.Path} Controller:{descriptor.ControllerName} Operation:{descriptor.ActionName}");

            await next();

            var message = JsonConvert.SerializeObject(new PermissionRequest
            {
                Id = Guid.NewGuid(),
                OperationName = descriptor.ActionName
            });

            await SendRequest(_settings.Value.Topic, message);
        }


        private async Task SendRequest(string topic, string message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _settings.Value.BootstrapServers,
                ClientId = Dns.GetHostName()
            };

            try
            {
                using var producer = new ProducerBuilder<Null, string>(config).Build();

                await producer.ProduceAsync(topic, new Message<Null, string>
                {
                    Value = message
                });
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred with Kafka");
            }
        }


    }
}
