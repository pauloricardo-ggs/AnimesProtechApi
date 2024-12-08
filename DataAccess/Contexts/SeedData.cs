using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts;
    
public static class SeedData
{
    public static void Seeding(this ModelBuilder builder)
    {
        builder.SeedRoles();
        builder.SeedAdmin();
    }

    public static void SeedRoles(this ModelBuilder builder)
    {
        builder.Entity<IdentityRole<Guid>>().HasData
        (
            new IdentityRole<Guid>
            {
                Id = Guid.Parse(RoleConstant.ADMIN_ID),
                Name = RoleConstant.ADMIN,
                NormalizedName = RoleConstant.ADMIN.ToUpper(),
            }
        );
    }

    private static void SeedAdmin(this ModelBuilder modelBuilder)
    {
        var adminId = Guid.Parse(ConfigurationEnvironment.GetEnvironmentVariable("ADMIN_USER_ID"));

        var admin = new User(
            id: adminId,
            email: ConfigurationEnvironment.GetEnvironmentVariable("ADMIN_USER_EMAIL"),
            emailConfirmed: true
        );

        var passwordHasher = new PasswordHasher<User>();
        admin.PasswordHash = passwordHasher.HashPassword(admin, ConfigurationEnvironment.GetEnvironmentVariable("ADMIN_USER_PASSWORD"));

        modelBuilder.Entity<User>()
            .HasData(admin);

        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .HasData(new IdentityUserRole<Guid> 
            { 
                RoleId = Guid.Parse(RoleConstant.ADMIN_ID),
                UserId = adminId 
            });
    }
}