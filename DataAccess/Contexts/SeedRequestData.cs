using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts;

public static class SeedRequestData
{
    public static void Seeding(this ModelBuilder modelBuilder)
    {
        modelBuilder.SeedRequisicaoTipo();
    }
    public static void SeedRequisicaoTipo(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RequestType>().HasData
        (
            new RequestType(
                new Guid(RequestTypeConstant.ANIME_CREATE_TYPE),
                RequestTypeConstant.ANIME_CREATE_NAME,
                RequestTypeConstant.ANIME_CREATE_PATH
            )
        );
    }
}