using Application.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models.Form
{
    public class FormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Add Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel? User { get; set; }
        public virtual ICollection<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
    }
}
