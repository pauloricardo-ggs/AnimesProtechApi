using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Mappings;

public class RoleMap : IEntityTypeConfiguration<IdentityRole<Guid>>
{
   public void Configure(EntityTypeBuilder<IdentityRole<Guid>> entity)
   {
      entity.ToTable("Roles");
   }
}
