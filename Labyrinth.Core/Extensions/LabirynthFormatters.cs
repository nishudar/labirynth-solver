
using LabyrinthCore.Data;

namespace LabyrinthCore.Extensions;

public static class LabirynthFormatterExtensions
{
    public static string GetMazeString(this Labyrinth labyrinth)
    {
        var result = new StringBuilder();
        foreach (var fieldLine in labyrinth.Fields)
        {
            foreach (var field in fieldLine) 
                result.Append(field.FieldType.ToFieldChar());
            result.AppendLine();
        }

        return result.ToString();
    }
    
    public static string GetPathString(this Labyrinth labyrinth)
    {
        var result = new StringBuilder();
        foreach (var fieldLine in labyrinth.Fields)
        {
            foreach (var field in fieldLine) 
                result.Append(labyrinth.IsPath(field) ? '*' : ' ');
            result.AppendLine();
        }

        return result.ToString();
    }
}