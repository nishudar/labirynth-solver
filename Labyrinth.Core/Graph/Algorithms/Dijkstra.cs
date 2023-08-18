namespace LabyrinthCore.Graph.Algorithms;

public class Dijkstra : IFindPathAlgorithm
{
    public List<Vertex<T>> FindShortestPath<T>(Graph<T> graph, Vertex<T> start, Vertex<T> end)
    {
        var previous = new Dictionary<Vertex<T>, Vertex<T>>();
        var distances = new Dictionary<Vertex<T>, int>();
        var unvisited = new HashSet<Vertex<T>>(graph.Vertices);

        foreach (var vertex in graph.Vertices)
            distances[vertex] = int.MaxValue;

        distances[start] = 0;
        while (unvisited.Any())
        {
            Vertex<T> current = null;
            foreach (var vertex in unvisited
                         .Where(vertex => current == null || distances[vertex] < distances[current]))
                current = vertex;

            if (current == end)
                break;

            unvisited.Remove(current);

            foreach (var neighbour in current!.NeighbourVertices)
            {
                var alt = distances[current] + 1;
                if (alt < distances[neighbour])
                {
                    distances[neighbour] = alt;
                    previous[neighbour] = current;
                }
            }
        }
        var path = new List<Vertex<T>>();
        for (var temp = end; temp != null; previous.TryGetValue(temp, out temp)) 
            path.Insert(0, temp);

        return path[0] == start ? path : null;
    }
}
