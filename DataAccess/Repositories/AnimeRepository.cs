using DataAccess.Contexts;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class AnimeRepository(ApplicationDbContext context) : RepositoryBase<Anime>(context), IAnimeRepository
{
    public async Task<ICollection<Anime>> List(string[]? filters)
    {
        return await context.Set<Anime>()
            .ApplyFilters(filters)
            .ToListAsync();
    }

    public async Task<Anime?> GetByName(string name)
    {
        return await context.Set<Anime>()
            .Where(anime => anime.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();
    }
}
