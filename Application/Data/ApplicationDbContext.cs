using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Application.Models.Form;
using Application.Models.User;
using Application.Models.Answer;

namespace Application.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {
        public DbSet<FormModel> Forms { get; set; }
        public DbSet<OptionModel> Options { get; set; }
        public DbSet<QuestionModel> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
