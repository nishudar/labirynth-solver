
using LabyrinthCore.Data;

namespace LabyrinthCore.Extensions;

public static class LabirynthFormatterExtensions
{
    public static string GetMazeString(this Labyrinth labyrinth)
    {
        var sb = new StringBuilder();
        foreach (var fieldLine in labyrinth.Fields)
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
    
    public static string GetPathString(this Labyrinth labyrinth)
    {
        var sb = new StringBuilder();
        foreach (var fieldLine in labyrinth.Fields)
        {
            foreach (var field in fieldLine)
            {
                var character = labyrinth.IsPath(field) ? '*' : ' ';
                sb.Append(character);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}