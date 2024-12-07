namespace Core.v1.Anime.Requests;

public class CreateAnimeRequest
{
    public required string Name { get; set; }
    public required string Summary { get; set; }
    public required string Director { get; set; }
}