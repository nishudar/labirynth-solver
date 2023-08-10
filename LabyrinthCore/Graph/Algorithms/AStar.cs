using System.Collections.Generic;
using System.Linq;

namespace LabyrinthCore.Graph.Algorithms;

public class AStar : IFindPathAlgorithm
{
    public List<Vertex<T>> FindShortestPath<T>(Graph<T> graphToSearch, Vertex<T> startingVertex, Vertex<T> targetVertex)
    {
        // List of vertices yet to be evaluated.
        var verticesToEvaluate = new List<Vertex<T>> { startingVertex };

        // Keeps track of the most efficient previous step to reach each vertex.
        var vertexPathMap = new Dictionary<Vertex<T>, Vertex<T>>();

        // For each vertex, this stores the cheapest known cost to reach it from the starting vertex.
        var distanceFromStart = graphToSearch.Vertices.ToDictionary(vertex => vertex, vertex => int.MaxValue);
        distanceFromStart[startingVertex] = 0;

        // For each vertex, the total cost estimated from start to target through that vertex.
        var estimatedTotalDistance = graphToSearch.Vertices.ToDictionary(vertex => vertex, vertex => int.MaxValue);
        estimatedTotalDistance[startingVertex] = HeuristicCostEstimate(startingVertex, targetVertex);

        // While there are vertices left to evaluate.
        while (verticesToEvaluate.Any())
        {
            // Choose the vertex with the lowest estimated cost to the target.
            var currentVertex = verticesToEvaluate.OrderBy(vertex => estimatedTotalDistance[vertex]).First();

            // If the current vertex is the target, the path has been found.
            if (currentVertex == targetVertex)
                return ReconstructPath(vertexPathMap, currentVertex);

            verticesToEvaluate.Remove(currentVertex);

            // For each neighboring vertex of the current vertex.
            foreach (var neighbourVertex in currentVertex.NeighbourVertices)
            {
                // Tentatively calculate the distance if taking the path through this neighbor.
                var tentativeDistance = distanceFromStart[currentVertex] + 1;  // Assuming edge weight is 1

                // If this tentative distance is shorter than previously known, update it.
                if (tentativeDistance < distanceFromStart[neighbourVertex])
                {
                    vertexPathMap[neighbourVertex] = currentVertex;
                    distanceFromStart[neighbourVertex] = tentativeDistance;
                    estimatedTotalDistance[neighbourVertex] = distanceFromStart[neighbourVertex] + HeuristicCostEstimate(neighbourVertex, targetVertex);

                    // Add the neighboring vertex to the list if it hasn't been evaluated yet.
                    if (!verticesToEvaluate.Contains(neighbourVertex))
                        verticesToEvaluate.Add(neighbourVertex);
                }
            }
        }

        return null;
    }

    private static List<Vertex<T>> ReconstructPath<T>(Dictionary<Vertex<T>, Vertex<T>> vertexPathMap, Vertex<T> currentVertex)
    {
        var shortestPath = new List<Vertex<T>> { currentVertex };
        while (vertexPathMap.ContainsKey(currentVertex))
        {
            currentVertex = vertexPathMap[currentVertex];
            shortestPath.Insert(0, currentVertex);
        }
        return shortestPath;
    }

    private static int HeuristicCostEstimate<T>(Vertex<T> sourceVertex, Vertex<T> destinationVertex) => 1;
}