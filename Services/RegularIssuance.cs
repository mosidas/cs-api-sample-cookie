namespace plain.Services;


/// <summary>
/// ポイントの定期発行
/// </summary>
public class RegularIssuance : CronJobService
{


    /// <summary>
    /// コントラクタ。
    /// </summary>
    /// <returns></returns>
    public RegularIssuance() : base("*/1 * * * *", TimeZoneInfo.Local)
    {
    }

    protected override Task DoWork(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}