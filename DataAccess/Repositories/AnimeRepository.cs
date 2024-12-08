using DataAccess.Contexts;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class AnimeRepository(ApplicationDbContext context) : RepositoryBase<Anime>(context), IAnimeRepository
{
    public async Task<PagedList<Anime>> List(string[]? filters, int page, int pageSize)
    {
        return await context.Set<Anime>()
            .ApplyFilters(filters)
            .ToPagedList(page, pageSize);
    }

    public async Task<Anime?> GetByName(string name)
    {
        return await context.Set<Anime>()
            .Where(anime => anime.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();
    }
}
