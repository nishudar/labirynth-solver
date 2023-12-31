﻿using LabyrinthCore.Graph.Algorithms;
using LabyrinthCore.Solver;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LabyrinthCore;
using LabyrinthCore.Data;
using LabyrinthCore.Extensions;

namespace Visualisation
{
    public partial class MainWindow : Window
    {
        public MainWindow() 
            => InitializeComponent();

        private void DrawMaze(object sender, RoutedEventArgs e)
        {
            try
            {
                MazeCanvas.Children.Clear();
                var labyrinth =
                    LabirynthExtensions.LoadFromText(MazeText.Text, new AStar(), new DoorKeyTeleportSolver());

                var fields = labyrinth.AllFields().ToArray();
                var size = (int) Math.Min(MazeCanvas.Width / labyrinth.Width, MazeCanvas.Height / labyrinth.Height);

                foreach (var field in fields)
                {
                    var fill = GetFill(field.FieldType, labyrinth.IsPath(field));
                    var rect = new Rectangle
                    {

                        Width = size,
                        Height = size,
                        Fill = fill,
                        RadiusX = (double) size / 2,
                        RadiusY = (double) size / 3
                    };
                    Canvas.SetLeft(rect, field.X * size);
                    Canvas.SetTop(rect, field.Y * size);

                    MazeCanvas.Children.Add(rect);

                    if (field.FieldType is FieldType.Empty or FieldType.Wall)
                        continue;

                    var text = new TextBlock
                    {
                        Text = field.FieldType.ToFieldChar().ToString(),
                        Foreground = System.Windows.Media.Brushes.Black,
                        FontSize = 10,
                        Margin = new Thickness(0),
                        Padding = new Thickness(0)
                    };

                    Canvas.SetLeft(text, Canvas.GetLeft(rect) + 5);
                    Canvas.SetTop(text, Canvas.GetTop(rect));
                    MazeCanvas.Children.Add(text);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private Brush GetFill(FieldType fieldType, bool isPath) 
            => fieldType switch
            {
                FieldType.Start => Brushes.Fuchsia,
                FieldType.End => Brushes.Green,
                FieldType.Door => Brushes.DeepPink,
                FieldType.Wall => Brushes.Silver,
                FieldType.Teleport => Brushes.Violet,
                FieldType.Key => Brushes.Gold,
                FieldType.Unknown => Brushes.Red,
                _ => isPath ? new SolidColorBrush(Colors.Blue) : Brushes.White
            };
    }
}
