using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamensArbete_BA_WIN23.API.Entities;

public enum NotificationType
{
    changeRequest = 0,
}

    
public class Notification
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("ChangeRequest")]
    public Guid ChangeRequestId { get; set; }
    public ChangeRequest? ChangeRequest { get; set; }
    public NotificationType Type { get; set; }
    public DateTimeOffset Created { get; set; }
    public bool isDeactivated { get; set; } = false;
    public string? Message { get; set; }
}