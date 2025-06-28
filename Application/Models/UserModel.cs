using Microsoft.AspNetCore.Identity;

namespace Application.Models
{
    public class UserModel : IdentityUser
    {
        public required string FullName { get; set; }
        public required bool IsBlocked { get; set; } = false;
    }
}
