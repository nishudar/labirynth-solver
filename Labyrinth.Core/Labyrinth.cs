using System;
using System.Collections.Generic;
using System.Linq;
using LabyrinthCore.Data;
using LabyrinthCore.Graph;
using LabyrinthCore.Graph.Algorithms;
using LabyrinthCore.Solver;

namespace LabyrinthCore;

public class Labyrinth
{
    public IReadOnlyList<IReadOnlyList<Field>> Fields { get; }
    public IReadOnlyCollection<Field> AllFields => Fields.SelectMany(line => line).ToList();
    public int Width => Fields.Min(line => line.Count);
    public int Height => Fields.Count;
    public Field? Start => AllFields.Single(f => f.FieldType == FieldType.Start);
    public Field? End => AllFields.Single(f => f.FieldType == FieldType.End);
    public bool IsPath(Field field) => Path?.Exists(v => v.Value == field)??false;
    public List<Vertex<Field>> Path { get; set; }

    public ISolver Solver { get; set; }
    
    public Labyrinth(IReadOnlyList<IReadOnlyList<Field>> fields, IFindPathAlgorithm algorithm, ISolver solver)
    {
        Fields = fields;
        if (Start == End)
            throw new ArgumentException("Start cannot be the end");
        
        Solver = solver;
        Solve(algorithm, solver);
    }

    private void Solve(IFindPathAlgorithm algorithm, ISolver solver) 
        => Path = solver.GetPath(algorithm, this);

    public IEnumerable<Field> GetNeighbours(Field field)
    {
        var fields = new[]
        {
            GetField(field.X - 1, field.Y),
            GetField(field.X, field.Y - 1),
            GetField(field.X + 1, field.Y),
            GetField(field.X, field.Y + 1),
        }.ToArray();
            
        return fields
            .Where(field1 => field1 is not null)
            .Select(field2=> field2!.Value)
            .ToArray();
    }
    
    private Field? GetField(int x, int y)
    {
        if (y >= Height || y < 0)
            return null;
        if (x >= Width || x < 0)
            return null;
        
        return Fields[y][x];
    }
}