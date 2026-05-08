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
            IsSelected = 1;
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
        private async void action2(object sender, RoutedEventArgs e)
        {
            if (myPB.Value > 0) { return; }
            myPB.Minimum = 0;
            myPB.Maximum = 100;
            for (int i = 0; i <= 100; i++)
            {
                myPB.Value = i;
                await Task.Delay(5);
            }
            myCanvas.Children.Clear();
            int count = myCanvas.Children.Count;
            //myCanvas.Children.RemoveAt(count-1);
            MessageBox.Show("Gotowe!");
            myPB.Value = 0;
        }
        private Point? _firstPoint = null;
        private async void PaintCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point currentPoint = e.GetPosition(myCanvas);

            if (_firstPoint == null)
            {
                _firstPoint = currentPoint;
                DrawPoint(currentPoint);
            }
            else
            {
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
                //rect.Name = "re";
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
            Canvas.SetLeft(dot, p.X - 2);
            Canvas.SetTop(dot, p.Y - 2);
            myCanvas.Children.Add(dot);
        }
        private void DrawPoint2(Point p)
        {
            Ellipse dot = new Ellipse { Width = 6, Height = 6, Fill = Brushes.Blue };
            Canvas.SetLeft(dot, p.X - 3);
            Canvas.SetTop(dot, p.Y - 3);
            myCanvas.Children.Add(dot);
        }
        public int IsSelected {  get; set; }
        private void MouseUP(object sender, MouseButtonEventArgs e)
        {
            Point currentPoint = e.GetPosition(myCanvas);
            DrawPoint2(currentPoint);
            IsSelected = -IsSelected;
        }

        private void MouseMV(object sender, MouseEventArgs e)
        {
            if(IsSelected == -1)
            {
                Point currentPoint = e.GetPosition(myCanvas);
                DrawPoint2(currentPoint);
            }

        }

        private void CreateSquare_Click(object sender, RoutedEventArgs e)
        {
            // 1. Tworzymy nowy obiekt prostokąta
            Rectangle newSquare = new Rectangle();
            newSquare.Width = 50;
            newSquare.Height = 50;
            newSquare.Fill = Brushes.Blue;

            // 2. Przypisujemy mu zdarzenie MouseDown (tak jak w XAML)
            newSquare.MouseDown += MySquare_MouseDown;

            // 3. Ustawiamy mu startową pozycję (np. na środku)
            Canvas.SetLeft(newSquare, 50);
            Canvas.SetTop(newSquare, 50);

            // 4. Dodajemy go do Canvasa
            myCanvas.Children.Add(newSquare);
        }

        // Ta metoda obsłuży KAŻDY stworzony kwadrat
        private void MySquare_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // 'sender' to obiekt, który właśnie został kliknięty
            Rectangle clickedSquare = sender as Rectangle;

            if (clickedSquare != null)
            {
                Random rnd = new Random();

                double maxX = myCanvas.ActualWidth - clickedSquare.Width;
                double maxY = myCanvas.ActualHeight - clickedSquare.Height;

                double newX = rnd.NextDouble() * maxX;
                double newY = rnd.NextDouble() * maxY;

                Canvas.SetLeft(clickedSquare, newX);
                Canvas.SetTop(clickedSquare, newY);
            }
        }
    }
}