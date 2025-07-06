namespace Application.Models.Answer
{
    public class FormResponseViewModel
    {
        public int FormId { get; set; }
        public List<AnswerElement> Answers { get; set; } = new();
    }
}
