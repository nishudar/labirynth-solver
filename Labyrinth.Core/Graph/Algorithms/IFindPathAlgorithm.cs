namespace LabyrinthCore.Graph.Algorithms;

public interface IFindPathAlgorithm
{
    List<Vertex<T>> FindShortestPath<T>(Graph<T> graph, Vertex<T> start, Vertex<T> end);
}