namespace Core.v1.Anime.Dtos;

public class AnimesDetailsDto
{
    public required IEnumerable<AnimeDetailsDto> Animes { get; set; }
}