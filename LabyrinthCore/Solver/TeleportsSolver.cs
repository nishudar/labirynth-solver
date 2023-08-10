using System;
using System.Collections.Generic;
using System.Linq;
using LabyrinthCore.Graph;

namespace LabyrinthCore.Solver;

public class TeleportsSolver : ISolver
{
    public List<Vertex<Field>> GetPath(IFindPathAlgorithm algorithm, Labyrinth labyrinth)
    {
        var graph = new Graph<Field>();

        var whitelistedFields = labyrinth.AllFields.Where(IsWhitelistedField()).ToArray();
        
        foreach (var field in whitelistedFields)
        {
            var isNew = graph.Vertices.All(v => v.Value != field);
            var vertex = graph.Vertices.FirstOrDefault(v => v.Value == field) ?? new Vertex<Field>(field);
            var directNeighbours = labyrinth.GetNeighbours(field);
            var fieldNeighbours = GetAccessibleNeighbours(directNeighbours).ToList();

            if (field.FieldType is FieldType.Teleport)
            {
                var otherTeleports = whitelistedFields.Where(accessibleField => accessibleField.FieldType is FieldType.Teleport && accessibleField != field);
                fieldNeighbours.AddRange(otherTeleports);
            }
            
            foreach (var neighbourField in fieldNeighbours)
            {
                var neighbourVertex = graph.Vertices.FirstOrDefault(v => v.Value == neighbourField);
                
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