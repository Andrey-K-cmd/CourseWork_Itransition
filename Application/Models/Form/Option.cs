namespace Application.Models.Form
{
    public class Option
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; } = new Question();
    }
}
