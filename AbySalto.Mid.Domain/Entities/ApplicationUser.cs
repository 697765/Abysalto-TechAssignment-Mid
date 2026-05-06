using Microsoft.AspNetCore.Identity;

namespace AbySalto.Mid.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
    }
}
