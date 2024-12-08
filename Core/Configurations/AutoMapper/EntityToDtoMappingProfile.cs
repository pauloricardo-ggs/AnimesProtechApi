using AutoMapper;
using Core.v1.Anime.Dtos;
using Domain.Entities;

namespace Core.Configurations.AutoMapper;

public class EntityToDtoMappingProfile : Profile
{
    public EntityToDtoMappingProfile()
    {
        #region Anime
        CreateMap<ICollection<Anime>, AnimesDetailsDto>()
            .ForMember(dto => dto.Animes, entity => entity.MapFrom(src => src));
        CreateMap<Anime, AnimeDetailsDto>();
        #endregion
    }
}