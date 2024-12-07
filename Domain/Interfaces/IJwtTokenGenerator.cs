using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces;

public interface IJwtTokenGenerator
{
    Task<Jwt> Generate(User user, UserManager<User> userManager);
}