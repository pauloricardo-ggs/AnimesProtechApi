using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IAnimeRepository : IRepositoryBase<Anime>
{
    Task<ICollection<Anime>> List(string[]? filters);
    Task<Anime?> GetByName(string name);
}
