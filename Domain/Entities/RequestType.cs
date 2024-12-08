namespace Domain.Entities;

public class RequestType
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Path { get; private set; }

    private List<Request> requests;
    public IEnumerable<Request> Requests => requests.AsReadOnly();

    public RequestType(Guid id, string name, string path)
        => (Id, Name, Path) = (id, name, path);

    // public RequisicaoTipo(string nome, string descricao, int quantidadeTentativa, int intervalo)
    //     => (Nome, Descricao, QuantidadeTentativa, Intervalo) = (nome, descricao, quantidadeTentativa, intervalo);
        
    // public RequisicaoTipo(Guid id, string nome, string descricao, int quantidadeTentativa, int intervalo)
    //     => (Id, Nome, Descricao, QuantidadeTentativa, Intervalo) = (id, nome, descricao, quantidadeTentativa, intervalo);
}