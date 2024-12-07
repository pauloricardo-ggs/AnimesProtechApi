using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Mappings;

public class UsersRolesMap : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
   public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> entity)
   {
      entity.ToTable("UsersRoles");
   }
}
