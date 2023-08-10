using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LabyrinthCore.Graph;
using LabyrinthCore.Solver;

namespace LabyrinthCore;

public class Labyrinth
{
    private readonly IFindPathAlgorithm _algorithm;
    public IReadOnlyList<IReadOnlyList<Field>> Fields { get; private set; }
    public IReadOnlyCollection<Field> AllFields => Fields.SelectMany(line => line).ToList();
    public int Width => Fields.Min(line => line.Count);
    public int Height => Fields.Count;
    public Field? Start => AllFields.Single(f => f.FieldType == FieldType.Start);
    public Field? End => AllFields.Single(f => f.FieldType == FieldType.End);
    public bool IsPath(Field field) => Path?.Exists(v => v.Value == field)??false;
    public List<Vertex<Field>>? Path { get; private set; } = null!;
    
    public ISolver Solver { get; set; }


    public Labyrinth(IReadOnlyList<IReadOnlyList<Field>> fields, IFindPathAlgorithm algorithm, ISolver solver)
    {
        _algorithm = algorithm;
        Fields = fields;
        if (Start == End)
            throw new ArgumentException("Start cannot be the end");
        
        this.Solver = solver;
        this.Path = solver.GetPath(algorithm, this);
    }

    public static Labyrinth LoadFromFile(string fileName, IFindPathAlgorithm algorithm, ISolver solver)
    {
        var fields = LoadFromFile(fileName);
        
        return new Labyrinth(fields, algorithm, solver);
    }

    public static Labyrinth LoadFromText(string text, IFindPathAlgorithm algorithm, ISolver solver)
    {
        var fields = LoadFromText(text);

        return new Labyrinth(fields, algorithm, solver);
    }

    public Field? GetField(int x, int y)
    {
        if (y >= Height || y < 0)
            return null;
        if (x >= Width || x < 0)
            return null;
        
        return Fields[y][x];
    }

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

    private static List<List<Field>> LoadFromText(string text)
    {
        var lines = text.Replace("\r", "").Split('\n');
        var minLength = lines.Min(line => line.Length);
        var result = new List<List<Field>>();
        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            var lineResult = new List<Field>();
            for (var x = 0; x < minLength; x++)
            {
                var field = new Field
                {
                    FieldType = line[x].ToFieldType(),
                    X = x,
                    Y = y
                };
                lineResult.Add(field);
            }
            result.Add(lineResult);
        }

        return result;
    }

    private static List<List<Field>> LoadFromFile(string fileName)
    {
        var lines = File.ReadLines(fileName).ToArray();
        var minLength = lines.Min(line => line.Length);
        var result = new List<List<Field>>();
        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            var lineResult = new List<Field>();
            for (var x = 0; x < minLength; x++)
            {
                var field = new Field
                {
                    FieldType = line[x].ToFieldType(),
                    X = x,
                    Y = y
                };
                lineResult.Add(field);                
            }
            result.Add(lineResult);
        }
        
        return result;
    }

    public string GetMazeString()
    {
        var sb = new StringBuilder();
        foreach (var fieldLine in Fields)
        {
            foreach (var field in fieldLine)
            {
                var character = field.FieldType.ToFieldChar();
                sb.Append(character);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
    
    public string GetPathString()
    {
        var sb = new StringBuilder();
        foreach (var fieldLine in Fields)
        {
            foreach (var field in fieldLine)
            {
                var character = IsPath(field) ? '*' : ' ';
                sb.Append(character);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}