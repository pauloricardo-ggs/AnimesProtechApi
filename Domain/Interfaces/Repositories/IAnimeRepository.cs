using Domain.Entities;
using Domain.Helpers;

namespace Domain.Interfaces.Repositories;

public interface IAnimeRepository : IRepositoryBase<Anime>
{
    Task<PagedList<Anime>> List(string[]? filters, int page, int pageSize);
    Task<Anime?> GetByName(string name);
}
