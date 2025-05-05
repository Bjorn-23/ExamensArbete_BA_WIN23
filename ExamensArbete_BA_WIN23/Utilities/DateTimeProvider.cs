namespace ExamensArbete_BA_WIN23.Utilities;

public class DateTimeProvider
{
    public DateTimeOffset UtcNow => Utc();

    private DateTimeOffset Utc()
    {
        return DateTimeOffset.UtcNow;
    }
}
