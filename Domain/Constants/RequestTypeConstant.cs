namespace Domain.Constants;

public static class RequestTypeConstant
{
    #region Anime
    public const string ANIME_CREATE_TYPE = "648a145c-908c-4dec-a2cf-7a1474a2ca74";
    public const string ANIME_CREATE_NAME = "Create Anime";
    public const string ANIME_CREATE_PATH = "Application.Handlers.AnimeCommandHandler.Handle(CreateAnimeCommand)";

    public const string ANIME_UPDATE_TYPE = "bf8ac0d7-9dfb-4890-876b-e39fc36739e7";
    public const string ANIME_UPDATE_NAME = "Update Anime";
    public const string ANIME_UPDATE_PATH = "Application.Handlers.AnimeCommandHandler.Handle(UpdateAnimeCommand)";

    public const string ANIME_DELETE_TYPE = "420b212b-7506-4776-b810-401f41c31465";
    public const string ANIME_DELETE_NAME = "Delete Anime";
    public const string ANIME_DELETE_PATH = "Application.Handlers.AnimeCommandHandler.Handle(DeleteAnimeCommand)";
    #endregion
}