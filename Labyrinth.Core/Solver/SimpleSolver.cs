using LabyrinthCore.Data;
using LabyrinthCore.Graph;
using LabyrinthCore.Graph.Algorithms;

namespace LabyrinthCore.Solver;

public class SimpleSolver : ISolver
{
    public List<Vertex<Field>> GetPath(IFindPathAlgorithm algorithm, Labyrinth labyrinth)
    {
        var graph = new Graph<Field>();

        var whitelistedFields = labyrinth.AllFields().Where(IsWhitelistedField()).ToArray();
        
        foreach (var field in whitelistedFields)
        {
            var isNew = graph.Vertices.TrueForAll(v => v.Value != field);
            var vertex = graph.Vertices.Find(v => v.Value == field) ?? new Vertex<Field>(field);
            var directNeighbours = labyrinth.GetNeighbours(field);
            var fieldNeighbours = GetAccessibleNeighbours(directNeighbours).ToList();
            
            foreach (var neighbourField in fieldNeighbours)
            {
                var neighbourVertex = graph.Vertices.Find(v => v.Value == neighbourField);
                
                if (neighbourVertex is not null)
                    vertex.ConnectWithNeighbour(neighbourVertex);
                else
                {
                    neighbourVertex = new Vertex<Field>(neighbourField);
                    vertex.ConnectWithNeighbour(neighbourVertex);
                    graph.AddVertex(neighbourVertex);
                }
            }
            
            if(isNew)
                graph.AddVertex(vertex);
        }

        var start = graph.Vertices.Single(v => v.Value.FieldType == FieldType.Start);
        var end = graph.Vertices.Single(v => v.Value.FieldType == FieldType.End);

        var path = algorithm.FindShortestPath(graph, start, end);

        return path;
    }

    private static readonly FieldType[] ForbiddenFields = {FieldType.Wall, FieldType.Unknown, FieldType.Door};
    private static Func<Field, bool> IsWhitelistedField() => field =>  !ForbiddenFields.Contains(field.FieldType);
    private IEnumerable<Field> GetAccessibleNeighbours(IEnumerable<Field> fieldNeighbours) => fieldNeighbours.Where(IsWhitelistedField());
}