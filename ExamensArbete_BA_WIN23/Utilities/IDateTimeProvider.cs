
namespace ExamensArbete_BA_WIN23.Utilities;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}