namespace ExamensArbete_BA_WIN23.Utilities;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => Utc();

    private DateTimeOffset Utc()
    {
        return DateTimeOffset.UtcNow;
    }
}
