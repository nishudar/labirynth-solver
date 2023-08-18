namespace LabyrinthCore.Graph;

public class Vertex<T>
{
    public T Value { get; set; }
    public IReadOnlyList<Vertex<T>> NeighbourVertices { get; }

    private readonly List<Vertex<T>> _neighbours;

    public Vertex(T value)
    {
        Value = value;
        _neighbours = new List<Vertex<T>>();
        NeighbourVertices = _neighbours.AsReadOnly();
    }

    public void ConnectWithNeighbour(Vertex<T> neighbour)
    {
        _neighbours.Add(neighbour);
        if (!neighbour.NeighbourVertices.Contains(this)) 
            neighbour.AddDirectionalNeighbour(neighbour);
    }

    private void AddDirectionalNeighbour(Vertex<T> neighbour) 
        => _neighbours.Add(neighbour);

    public override string ToString() 
        => $"{nameof(Value)}: {Value}";
}
