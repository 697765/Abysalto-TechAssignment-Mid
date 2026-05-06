using AbySalto.Mid.Domain.Entities;
using AbySalto.Mid.WebApi.Models;

namespace AbySalto.Mid.WebApi.Mappers
{
    internal static class UserMapper
    {
        public static ApplicationUser ToDomain(RegistrationRequest dto) => new()
        {
            Name = dto.Name,
            Surname = dto.Surname,
            UserName = dto.Username,
            Email = dto.Email,
        };
    }
}