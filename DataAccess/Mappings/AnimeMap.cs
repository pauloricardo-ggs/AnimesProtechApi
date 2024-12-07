using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Mappings;

public class AnimeMap : IEntityTypeConfiguration<Anime>
{
   public void Configure(EntityTypeBuilder<Anime> entity)
   {
      entity.ToTable("Animes");

      entity.Property(anime => anime.Id)
         .HasColumnName("AnimeId")
         .IsRequired();

      entity.Property(anime => anime.Name)
         .HasColumnName("Name")
         .HasMaxLength(100)
         .IsRequired();

      entity.Property(anime => anime.Summary)
         .HasColumnName("Summary")
         .HasMaxLength(500)
         .IsRequired();

      entity.Property(anime => anime.Director)
         .HasColumnName("Director")
         .HasMaxLength(100)
         .IsRequired();
   }
}
