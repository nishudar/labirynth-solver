namespace LabyrinthCore.Graph.Algorithms;

public class AStar : IFindPathAlgorithm
{
    public List<Vertex<T>> FindShortestPath<T>(Graph<T> graph, Vertex<T> start, Vertex<T> end)
    {
        var verticesToEvaluate = new List<Vertex<T>> { start };
        var vertexPathMap = new Dictionary<Vertex<T>, Vertex<T>>();
        var distanceFromStart = graph.Vertices.ToDictionary(vertex => vertex, _ => int.MaxValue);
        distanceFromStart[start] = 0;

        var estimatedTotalDistance = graph.Vertices.ToDictionary(vertex => vertex, _ => int.MaxValue);
        estimatedTotalDistance[start] = HeuristicCostEstimate(start, end);

        while (verticesToEvaluate.Any())
        {
            var currentVertex = verticesToEvaluate.OrderBy(vertex => estimatedTotalDistance[vertex]).First();

            if (currentVertex == end)
                return ReconstructPath(vertexPathMap, currentVertex);

            verticesToEvaluate.Remove(currentVertex);

            foreach (var neighbourVertex in currentVertex.NeighbourVertices)
            {
                var tentativeDistance = distanceFromStart[currentVertex] + 1;

                if (tentativeDistance < distanceFromStart[neighbourVertex])
                {
                    vertexPathMap[neighbourVertex] = currentVertex;
                    distanceFromStart[neighbourVertex] = tentativeDistance;
                    estimatedTotalDistance[neighbourVertex] = distanceFromStart[neighbourVertex] + HeuristicCostEstimate(neighbourVertex, end);

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