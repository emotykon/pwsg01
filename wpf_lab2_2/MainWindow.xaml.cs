using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf_lab2_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Dane> Observable { get; set; }
        public class Dane
        {
            public int n1 { get; set; }
            public int n2 { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            Observable = new ObservableCollection<Dane>();
            Observable.Add(new Dane { n1 = 1, n2 = 2 });
            Observable.Add(new Dane { n1 = 3 , n2 = 4 });
            this.DataContext = this;
        }

        private void EXIT(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void action1(object sender, RoutedEventArgs e)
        {
            string input = CInput.Text;
            if (double.TryParse(input, out double coordinate))
            {
                DrawCircle(coordinate, coordinate);
            }
            else
            {
                MessageBox.Show("Wpisz poprawną liczbę!");
            }
        }
        private void DrawCircle(double x, double y)
        {
            Ellipse circle = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Green
            };

            Canvas.SetLeft(circle, x);
            Canvas.SetTop(circle, y);

            myCanvas.Children.Add(circle);
        }
        private void action2(object sender, RoutedEventArgs e)
        {

        }
        private Point? _firstPoint = null;
        private async void PaintCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            // Pobieramy aktualną pozycję myszy względem Canvas
            Point currentPoint = e.GetPosition(myCanvas);

            if (_firstPoint == null)
            {
                // KROK 1: To jest pierwsze kliknięcie
                _firstPoint = currentPoint;

                // Opcjonalnie: narysuj małą kropkę w miejscu pierwszego kliknięcia
                DrawPoint(currentPoint);
            }
            else
            {

                // KROK 2: To jest drugie kliknięcie - rysujemy linię
                Line line = new Line
                {
                    X1 = _firstPoint.Value.X,
                    Y1 = _firstPoint.Value.Y,
                    X2 = currentPoint.X,
                    Y2 = currentPoint.Y,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                Point p1 = _firstPoint.Value;
                Point p2 = currentPoint;
                double left = Math.Min(p1.X, p2.X);
                double top = Math.Min(p1.Y, p2.Y);
                double width = Math.Abs(p1.X - p2.X);
                double height = Math.Abs(p1.Y - p2.Y);
                Rectangle rect = new Rectangle
                {
                    Width = width,
                    Height = height,
                    Stroke = Brushes.Blue,
                    StrokeThickness = 2,
                    Fill = new SolidColorBrush(Color.FromArgb(50, 0, 0, 255)),
                    
                };
                Canvas.SetLeft(rect, left);
                Canvas.SetTop(rect, top);
                //rect.Opacity = 23;
                myCanvas.Children.Add(line);
                await Task.Delay(1000);
                myCanvas.Children.Add(rect);

                _firstPoint = null;
            }

        }
        private void DrawPoint(Point p)
        {
            Ellipse dot = new Ellipse { Width = 4, Height = 4, Fill = Brushes.Red };
            Canvas.SetLeft(dot, p.X - 2); // -2 aby wycentrować kropkę
            Canvas.SetTop(dot, p.Y - 2);
            myCanvas.Children.Add(dot);
        }
    }
}