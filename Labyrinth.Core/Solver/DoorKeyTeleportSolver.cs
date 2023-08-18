using LabyrinthCore.Data;
using LabyrinthCore.Graph;
using LabyrinthCore.Graph.Algorithms;

namespace LabyrinthCore.Solver;

public class DoorKeyTeleportSolver : ISolver
{
    public List<Vertex<Field>> GetPath(IFindPathAlgorithm algorithm, Labyrinth labyrinth)
    {
        List<Vertex<Field>> alternativePath = null;
        
        if (labyrinth.Start == null || labyrinth.End == null)
            return null;
        
        var path= SolveWithWithDoor(algorithm, labyrinth, false, labyrinth.Start.Value, labyrinth.End.Value);
        if (path is not null)
            alternativePath = path;
        
        List<Vertex<Field>> pathToKey = null;
        var keys = labyrinth.AllFields().Where(f => f.FieldType == FieldType.Key).ToArray();
        foreach (var key in keys)
        {
            pathToKey = SolveWithWithDoor(algorithm, labyrinth, false,  labyrinth.Start.Value,  key);
            if (pathToKey is not null)
                break;
        }
         
        if (pathToKey is null) 
            return alternativePath;
        
        var pathFromKeyToEnd =  SolveWithWithDoor(algorithm, labyrinth, true, pathToKey[pathToKey.Count - 1].Value, labyrinth.End.Value);

        List<Vertex<Field>> pathWithKey;
        if(pathFromKeyToEnd is not null)
        {
            pathWithKey = new List<Vertex<Field>>();
            pathWithKey.AddRange(pathToKey);
            pathWithKey.AddRange(pathFromKeyToEnd);
        }
        else
            return alternativePath;
        
        if (alternativePath == null)
            return pathWithKey;
        else
            return pathWithKey.Count < alternativePath.Count ?  pathWithKey : alternativePath;
    }
    
    private List<Vertex<Field>> SolveWithWithDoor(IFindPathAlgorithm algorithm, Labyrinth labyrinth, bool canPassDor, Field startField, Field endField)
    {
        var graph = new Graph<Field>();

        var whitelistedFields = labyrinth.AllFields().Where(IsWhitelistedField(canPassDor)).ToList();
        whitelistedFields.Remove(startField);
        whitelistedFields.Insert(0, startField);
        
        foreach (var field in whitelistedFields)
        {
            var isNew = graph.Vertices.TrueForAll(v => v.Value != field);
            var vertex = graph.Vertices.Find(v => v.Value == field) ?? new Vertex<Field>(field);
            var directNeighbours = labyrinth.GetNeighbours(field);
            var fieldNeighbours = GetAccessibleNeighbours(directNeighbours, canPassDor).ToList();

            if (field.FieldType is FieldType.Teleport)
            {
                var otherTeleports = whitelistedFields.Where(accessibleField => accessibleField.FieldType is FieldType.Teleport && accessibleField != field);
                fieldNeighbours.AddRange(otherTeleports);
            }
            
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
        
        

        Vertex<Field> start = graph.Vertices.First(v => v.Value == startField);
        Vertex<Field> end = graph.Vertices.First(v => v.Value == endField);
        
        var path = algorithm.FindShortestPath(graph, start, end);

        return path;
    }


    private static readonly FieldType[] ForbiddenFields = {FieldType.Wall, FieldType.Unknown, FieldType.Door};
    private static Func<Field, bool> IsWhitelistedField(bool canPassDor)=> field =>  (field.FieldType is FieldType.Door && canPassDor) || !ForbiddenFields.Contains(field.FieldType);
    private IEnumerable<Field> GetAccessibleNeighbours(IEnumerable<Field> fieldNeighbours, bool canUseDoor) => fieldNeighbours.Where(IsWhitelistedField(canUseDoor));
}