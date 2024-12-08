using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Application.Queries;

public class AnimeQueries(IAnimeRepository animeRepository) : IAnimeQueries
{
    private readonly IAnimeRepository _animeRepository = animeRepository;

    public async Task<Anime?> GetById(Guid id)
    {
        return await _animeRepository.GetByIdAsync(id);
    }

    public async Task<Anime?> GetByName(string name)
    {
        return await _animeRepository.GetByName(name);
    }

    public async Task<ICollection<Anime>> List(string[]? filters)
    {
        return await _animeRepository.List(filters);
    }
}