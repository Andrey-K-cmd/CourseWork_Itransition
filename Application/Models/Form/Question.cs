using Application.Enums;

namespace Application.Models.Form
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        public int FormId { get; set; }
        public virtual Form Form { get; set; } = new Form();
        public virtual ICollection<Option> Options { get; set; } = new List<Option>();
    }
}
