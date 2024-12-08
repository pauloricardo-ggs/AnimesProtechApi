using AutoMapper;
using Core.v1.Anime.Dtos;
using Domain.Entities;
using Domain.Helpers;

namespace Core.Configurations.AutoMapper;

public class EntityToDtoMappingProfile : Profile
{
    public EntityToDtoMappingProfile()
    {
        CreateMap(typeof(PagedList<>), typeof(PagedList<>))
            .ConvertUsing(typeof(PagedListConverter<,>));
            
        #region Anime
        CreateMap<Anime, AnimeDetailsDto>();
        #endregion
    }
}

public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
{
    public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
    {
        var mappedItems = context.Mapper.Map<List<TDestination>>(source.Items);
        return new PagedList<TDestination>(mappedItems, source.PageNumber, source.PageSize, source.TotalItemCount);
    }
}