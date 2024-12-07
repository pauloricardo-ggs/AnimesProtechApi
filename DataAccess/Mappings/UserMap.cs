using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
   public void Configure(EntityTypeBuilder<User> entity)
   {
      entity.ToTable("Users");
   }
}
