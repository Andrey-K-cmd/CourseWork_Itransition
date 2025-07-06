using Application.Enums;

namespace Application.Models.Form
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        public int FormId { get; set; }
        public virtual FormModel? Form { get; set; }
        public virtual ICollection<OptionModel> Options { get; set; } = new List<OptionModel>();
    }
}
