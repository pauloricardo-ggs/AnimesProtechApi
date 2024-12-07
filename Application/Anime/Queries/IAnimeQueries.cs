using Domain.Entities;

namespace Application.Queries;

public interface IAnimeQueries
{
    Task<Anime?> GetByName(string name);
}