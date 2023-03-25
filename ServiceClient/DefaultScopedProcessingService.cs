using Contracts;
using MassTransit;

namespace ServiceClient;

public sealed class DefaultScopedProcessingService : IScopedProcessingService
{
    private int _executionCount;
    private readonly ILogger<DefaultScopedProcessingService> _logger;
    private readonly IRequestClient<ServiceManagerRequest> _client;

    public DefaultScopedProcessingService(
        ILogger<DefaultScopedProcessingService> logger, IRequestClient<ServiceManagerRequest> client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            ++ _executionCount;

            _logger.LogInformation(
                "{ServiceName} working, execution count: {Count}",
                nameof(DefaultScopedProcessingService),
                _executionCount);

            var response = await _client.GetResponse<ServiceManagerResult>(new ServiceManagerRequest{ Action= $"OK - {DateTime.Now}"}, stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}