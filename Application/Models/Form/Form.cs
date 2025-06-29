namespace Application.Models.Form
{
    public class Form
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
