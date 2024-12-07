using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IAnimeRepository : IRepositoryBase<Anime>
{
    Task<Anime?> GetByName(string name);
}
