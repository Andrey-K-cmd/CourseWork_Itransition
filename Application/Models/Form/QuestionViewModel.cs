using Application.Enums;

namespace Application.Models.Form
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        public int FormId { get; set; }
        public virtual ICollection<OptionViewModel> Options { get; set; } = new List<OptionViewModel>();
    }
}
