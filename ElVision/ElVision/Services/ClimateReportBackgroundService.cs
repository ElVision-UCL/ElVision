using ElVision.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ElVision.Services;
public class ClimateReportBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ClimateReportBackgroundService> logger;
    private readonly IClimateReportService climateReportService;

    public ClimateReportBackgroundService(IServiceProvider serviceProvider, ILogger<ClimateReportBackgroundService> logger, IClimateReportService climateReportService)
    {
        _serviceProvider = serviceProvider;
        this.logger = logger;
        this.climateReportService = climateReportService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // This loop runs the task until the application is stopping
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    await climateReportService.GetClimateReportAsync();
                    logger.LogInformation("Climate Report Task has run at {Time}", DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Climate Report Task failed");
            }

            DateTime now = DateTime.Now;
            DateTime midnight = now.Date.AddDays(1);
            TimeSpan timeUntilMidnight = midnight - now;
            await Task.Delay(timeUntilMidnight, stoppingToken);
        }
    }
}
