namespace LabyrinthCore.Graph;

public class Graph<T>
{
    public List<Vertex<T>> Vertices { get; } = new();

    public Vertex<T> AddVertex(Vertex<T> vertex)
    {
        Vertices.Add(vertex);
        
        return vertex;
    }
}
