using Application.Models.Form;
using Application.Models.User;

namespace Application.Models.Answer
{
    public class Answer
    {
        public int Id { get; set; }

        public int FormId { get; set; }

        public FormModel Form { get; set; } = null!;

        public int QuestionId { get; set; }

        public QuestionModel Question { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;

        public virtual UserModel? User { get; set; }

        public string AnswerText { get; set; } = string.Empty;
    }

}
