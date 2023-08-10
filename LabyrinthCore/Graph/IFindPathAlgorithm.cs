using System.Collections.Generic;

namespace LabyrinthCore.Graph;

public interface IFindPathAlgorithm
{
    List<Vertex<T>>? FindShortestPath<T>(Graph<T> graph, Vertex<T> start, Vertex<T> end);
}