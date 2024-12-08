namespace Domain.Entities;

public class Anime(string name, string summary, string director)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public string Summary { get; private set; } = summary;
    public string Director { get; private set; } = director;

    public void Update(string name, string summary, string director)
        => (Name, Summary, Director) = (name, summary, director);
}