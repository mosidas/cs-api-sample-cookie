using Cronos;

namespace plain.Services;

/// <summary>
/// ref: https://github.com/dotnet-labs/ServiceWorkerCronJob/blob/master/ServiceWorkerCronJobDemo/Services/CronJobService.cs
/// </summary>
public abstract class CronJobService : BackgroundService
{
    private System.Timers.Timer? _timer;
    private readonly CronExpression _expression;
    private readonly TimeZoneInfo _timeZoneInfo;
    private CancellationTokenSource _cancellationTokenSource;

    protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo)
    {
        _expression = CronExpression.Parse(cronExpression);
        _timeZoneInfo = timeZoneInfo;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.Register(() => _cancellationTokenSource.Cancel());
        await ScheduleJob(_cancellationTokenSource.Token);
    }

    protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
    {
        var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);

        if (next.HasValue)
        {
            var delay = next.Value - DateTimeOffset.Now;
            if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into Timer
            {
                await ScheduleJob(cancellationToken);
            }
            _timer = new System.Timers.Timer(delay.TotalMilliseconds);
            _timer.Elapsed += async (sender, args) =>
            {
                _timer?.Dispose();
                _timer = null;

                if (!cancellationToken.IsCancellationRequested)
                {
                    await DoWork(cancellationToken);
                }

                if (!cancellationToken.IsCancellationRequested)
                {
                    await ScheduleJob(cancellationToken);
                }
            };
            _timer.Start();
        }
        await Task.CompletedTask;
    }

    protected abstract Task DoWork(CancellationToken cancellationToken);

    public async Task StartAsync()
    {
        if (_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }
        await ScheduleJob(_cancellationTokenSource.Token);
    }

    public async Task StopAsync()
    {
        _cancellationTokenSource.Cancel();
        _timer?.Stop();
        await Task.CompletedTask;
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this);
        base.Dispose();
    }
}
