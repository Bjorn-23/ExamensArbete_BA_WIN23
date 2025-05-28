namespace ExamensArbete_BA_WIN23.Business.Dtos;

public class ChangeRequestDto
{
    public int Id { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Updated { get; set; }
    public bool isConfirmed { get; set; }


}
