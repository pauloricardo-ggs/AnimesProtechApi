using Application.Commands;
using AutoMapper;
using Core.v1.Anime.Requests;
using Core.v1.Auth.Requests;

namespace Core.Configurations.AutoMapper;

public class RequestToCommandMappingProfile : Profile
{
    public RequestToCommandMappingProfile()
    {
        #region Auth
        CreateMap<SigninRequest, SigninCommand>();
        CreateMap<SignupRequest, SignupCommand>();
        #endregion

        #region Anime
        CreateMap<CreateAnimeRequest, CreateAnimeCommand>();
        #endregion
    }
}