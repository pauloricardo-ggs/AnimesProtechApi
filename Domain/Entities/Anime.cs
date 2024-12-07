namespace Domain.Entities;

public class Anime(string name, string summary, string director)
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = name;
    public string Summary { get; private set; } = summary;
    public string Director { get; private set; } = director;
}