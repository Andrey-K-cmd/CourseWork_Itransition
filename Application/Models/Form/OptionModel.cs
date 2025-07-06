using Application.Enums;

namespace Application.Models.Form
{
    public class OptionModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int QuestionId { get; set; }
        public virtual QuestionModel? Question { get; set; }
    }
}
