using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Application.Models.Form;
using Application.Models.User;

namespace Application.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {
        public DbSet<FormViewModel> Forms { get; set; }
        public DbSet<QuestionViewModel> Questions { get; set; }
        public DbSet<OptionViewModel> Options { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
