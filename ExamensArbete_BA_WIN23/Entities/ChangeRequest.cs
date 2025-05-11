using System.ComponentModel.DataAnnotations;

namespace ExamensArbete_BA_WIN23.API.Entities;

public class ChangeRequest
{
    [Key]
    public Guid ChangeRequestId { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Updated { get; set; }
    public int Customer { get; set; }
    public int Region { get; set; }
    public Guid? FirstApprover { get; set; }
    public Guid? SecondApprover { get; set; }
    public bool isSignCompleted { get; set; }
    public DateTimeOffset? DateSentBV { get; set; }
}
