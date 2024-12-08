using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Mappings;

public class RequestMap : IEntityTypeConfiguration<Request>
{
   public void Configure(EntityTypeBuilder<Request> entity)
   {
      entity.ToTable(LoggerConstant.REQUEST_TABLE_NAME);

      entity.Property(request => request.Id)
            .HasColumnName(LoggerConstant.PROPERTY_REQUEST_ID);

      entity.Property(request => request.Url)
            .IsRequired()
            .HasColumnName(LoggerConstant.PROPERTY_URL)
            .HasMaxLength(100);

      entity.Property(request => request.RequestData)
            .IsRequired()
            .HasColumnName(LoggerConstant.PROPERTY_REQUEST_DATA);

      entity.Property(request => request.ResponseData)
            .HasColumnName(LoggerConstant.PROPERTY_RESPONSE_DATA);

      entity.Property(request => request.HttpCode)
            .HasColumnName(LoggerConstant.PROPERTY_HTTP_CODE);

      entity.Property(request => request.LogDate)
            .IsRequired()
            .HasColumnName(LoggerConstant.PROPERTY_LOG_DATE);
      
      entity.Property(request => request.RequestTypeId)
               .IsRequired()
               .HasColumnName(LoggerConstant.PROPERTY_REQUEST_TYPE_ID);

      entity.HasOne(request => request.RequestType)
            .WithMany(requestType => requestType.Requests)
            .HasForeignKey(request => request.RequestTypeId);
   }
}
