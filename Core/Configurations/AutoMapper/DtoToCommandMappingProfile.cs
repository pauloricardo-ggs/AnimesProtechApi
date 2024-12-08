using Application.Commands;
using AutoMapper;
using Core.v1.Anime.Dtos;
using Core.v1.Auth.Dtos;

namespace Core.Configurations.AutoMapper;

public class DtoToCommandMappingProfile : Profile
{
    public DtoToCommandMappingProfile()
    {
        #region Auth
        CreateMap<SigninDto, SigninCommand>();
        CreateMap<SignupDto, SignupCommand>();
        #endregion

        #region Anime
        CreateMap<AnimeDto, CreateAnimeCommand>();
        CreateMap<AnimeDto, UpdateAnimeCommand>();
        #endregion
    }
}