using System.IO;
using LabyrinthCore.Data;
using LabyrinthCore.Graph.Algorithms;
using LabyrinthCore.Solver;

namespace LabyrinthCore.Extensions;

public static class LabirynthExtensions
{
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

}