using LabyrinthCore.Graph.Algorithms;
using LabyrinthCore.Solver;
using LabyrinthCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Visualisation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void OnDrawClick(object sender, RoutedEventArgs e)
        {
            Canv.Children.Clear();
            var labyrinth = Labyrinth.LoadFromText(labirynthText.Text, new AStar(), new DoorKeyTeleportSolver());
            var fields = labyrinth.AllFields.ToArray();
            int size = (int) Math.Min(Canv.Width/labyrinth.Width, Canv.Height/labyrinth.Height);

            for (int i = 0; i < fields.Length; i++)
            {
                var fill = GetFill(fields[i].FieldType, labyrinth.IsPath(fields[i]));
                var rect = new Rectangle
                {

                    Width = size,
                    Height = size,
                    Fill = fill,
                    RadiusX = size / 2,
                    RadiusY = size / 3
                };
                Canvas.SetLeft(rect, fields[i].X * size);
                Canvas.SetTop(rect, fields[i].Y * size);

                Canv.Children.Add(rect);

                if (fields[i].FieldType is not FieldType.Empty && fields[i].FieldType is not FieldType.Wall)
                {
                    TextBlock text = new TextBlock
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
        }

        Brush fuchsia = new SolidColorBrush(Colors.Fuchsia);
        Brush deepPink = new SolidColorBrush(Colors.DeepPink);
        Brush silver = new SolidColorBrush(Colors.Silver);
        Brush violetchia = new SolidColorBrush(Colors.Violet);
        Brush gold = new SolidColorBrush(Colors.Gold);
        Brush white = new SolidColorBrush(Colors.White);
        Brush green = new SolidColorBrush(Colors.White);
        Brush red = new SolidColorBrush(Colors.Red);

        private Brush GetFill(FieldType fieldType, bool isPath)
        {
            switch (fieldType)
            {
                case FieldType.Start:
                    return fuchsia;
                case FieldType.End:
                    return green;
                case FieldType.Door:
                    return deepPink;
                case FieldType.Wall:
                    return silver;
                case FieldType.Teleport:
                    return violetchia;
                case FieldType.Key:
                    return gold;
                case FieldType.Unknown:
                    return red;
            }                
            if(isPath)
                return new SolidColorBrush(Colors.Blue);

            return white;
        }

    }
}
