
using Microsoft.AspNetCore.Identity;
using TechProject.Models;

namespace Tech.Models.ViewModels
{
    public class UserVM
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
        public List<IdentityRole> AllRoles { get; set; }

    }
}
