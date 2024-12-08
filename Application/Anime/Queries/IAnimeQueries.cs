using Domain.Entities;

namespace Application.Queries;

public interface IAnimeQueries
{
    Task<Anime?> GetById(Guid id);
    Task<Anime?> GetByName(string name);
    Task<ICollection<Anime>> List(string[]? filters);
}