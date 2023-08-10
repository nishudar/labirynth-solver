using LabyrinthCore.Graph.Algorithms;
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
        {
            InitializeComponent();
        }

        private void OnDrawClick(object sender, RoutedEventArgs e)
        {
            Canv.Children.Clear();
            Labyrinth labyrinth;
            try
            {
                labyrinth = LabirynthExtensions.LoadFromText(labirynthText.Text, new AStar(), new DoorKeyTeleportSolver());
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Cannot resole maze");
                return;
            }

            var fields = labyrinth.AllFields.ToArray();
            var size = (int) Math.Min(Canv.Width/labyrinth.Width, Canv.Height/labyrinth.Height);

            for (var i = 0; i < fields.Length; i++)
            {
                var fill = GetFill(fields[i].FieldType, labyrinth.IsPath(fields[i]));
                var rect = new Rectangle
                {

                    Width = size,
                    Height = size,
                    Fill = fill,
                    RadiusX = (double)size/2,
                    RadiusY = (double)size/3
                };
                Canvas.SetLeft(rect, fields[i].X * size);
                Canvas.SetTop(rect, fields[i].Y * size);

                Canv.Children.Add(rect);

                if (fields[i].FieldType is FieldType.Empty || fields[i].FieldType is FieldType.Wall) 
                    continue;
                
                var text = new TextBlock
                {
                    Text = fields[i].FieldType.ToFieldChar().ToString(),
                    Foreground = Brushes.Black,
                    FontSize = 10,
                    Margin = new Thickness(0),
                    Padding = new Thickness(0)
                };

                Canvas.SetLeft(text, Canvas.GetLeft(rect) + 5);
                Canvas.SetTop(text, Canvas.GetTop(rect));
                Canv.Children.Add(text);
            }
        }

        private Brush GetFill(FieldType fieldType, bool isPath)
        {
            switch (fieldType)
            {
                case FieldType.Start:
                    return Brashes.Fuchsia;
                case FieldType.End:
                    return Brashes.Green;
                case FieldType.Door:
                    return Brashes.DeepPink;
                case FieldType.Wall:
                    return Brashes.Silver;
                case FieldType.Teleport:
                    return Brashes.Violetchia;
                case FieldType.Key:
                    return Brashes.Gold;
                case FieldType.Unknown:
                    return Brashes.Red;
            }                
            return isPath ? new SolidColorBrush(Colors.Blue) : Brashes.White;
        }

    }
}
