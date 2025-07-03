namespace Application.Models.Form
{
    public class OptionViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int QuestionId { get; set; }
        public virtual QuestionViewModel Question { get; set; } = new QuestionViewModel();
    }
}
