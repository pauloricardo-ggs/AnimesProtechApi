using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Mappings;

public class RequestTypeMap : IEntityTypeConfiguration<RequestType>
{
      public void Configure(EntityTypeBuilder<RequestType> entity)
      {
      entity.ToTable("RequestType");
      entity.Property(requestType => requestType.Id)
            .HasColumnName("RequestTypeId");

      entity.Property(requestType => requestType.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasMaxLength(100);

      entity.Property(requestType => requestType.Path)
            .IsRequired()
            .HasColumnName("Path")
            .HasMaxLength(100);
      }
}