using System.Text;
using LabyrinthCore;
using LabyrinthCore.Data;
using LabyrinthCore.Extensions;
using LabyrinthCore.Graph.Algorithms;
using LabyrinthCore.Solver;

Console.WriteLine("Loading input.txt");
var labyrinth = LabirynthExtensions.LoadFromFile("input.txt", new AStar(), new DoorKeyTeleportSolver());
Console.WriteLine();

Console.WriteLine(labyrinth.GetMazeString());
Console.WriteLine();
Console.WriteLine();
Console.WriteLine(labyrinth.GetPathString());
Console.WriteLine();
Console.WriteLine();
DrawLabyrinth(labyrinth);
Console.WriteLine();
Console.WriteLine(string.Join(";", labyrinth?.Path?.Select(p => p.Value.ToString()??"Not found")!));


void DrawLabyrinth(Labyrinth labyrinth)
{
    var sb = new StringBuilder();
    foreach (var fieldLine in labyrinth.Fields)
    {
        foreach (var field in fieldLine)
        {
            if(!labyrinth.IsPath(field) || field.FieldType is FieldType.Start or FieldType.End or FieldType.Teleport or FieldType.Door or FieldType.Key)
            {
                var color = field.FieldType switch
                {
                    FieldType.Unknown => ConsoleColor.Red,
                    FieldType.Start => ConsoleColor.DarkCyan,
                    FieldType.End => ConsoleColor.Green,
                    FieldType.Wall => ConsoleColor.DarkGray,
                    FieldType.Empty => ConsoleColor.White,
                    FieldType.Teleport => ConsoleColor.DarkMagenta,
                    FieldType.Door => ConsoleColor.DarkBlue,
                    FieldType.Key => ConsoleColor.Yellow,
                    _ => throw new ArgumentOutOfRangeException()
                };
                Console.ForegroundColor = color;
                Console.Write(field.FieldType.ToFieldChar());
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write('*');
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        Console.WriteLine();
    }
}