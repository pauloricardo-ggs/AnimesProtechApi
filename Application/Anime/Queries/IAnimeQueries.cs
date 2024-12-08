using Domain.Entities;
using Domain.Helpers;

namespace Application.Queries;

public interface IAnimeQueries
{
    Task<Anime?> GetById(Guid id);
    Task<Anime?> GetByName(string name);
    Task<PagedList<Anime>> List(string[]? filters, int page, int pageSize);
}