using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Application.Queries;

public class AnimeQueries(IAnimeRepository animeRepository) : IAnimeQueries
{
    private readonly IAnimeRepository _animeRepository = animeRepository;

    public async Task<Anime?> GetByName(string name)
    {
        return await _animeRepository.GetByName(name);
    }
}