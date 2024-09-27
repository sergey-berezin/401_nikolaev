using System;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// using GeneticAlgorithm;
using GeneticSquares;

namespace GeneticApplication
{
    public partial class MainWindow : Window
    {
        public class Rect(double x, double y, double w, double h, string color)
        {
            public double X { get; set; } = x;
            public double Y { get; set; } = y;
            public double Width { get; set; } = w;
            public double Height { get; set; } = h;
            public string Color { get; set; } = color;
        }
        public class Data(int a, int b, int c)
        {
            public int A { get; set; } = a;
            public int B { get; set; } = b;
            public int C { get; set; } = c;

            public int TotalWidth { get; set; }
            public int TotalHeight { get; set; }
            public int TotalSquare { get; set; }
            public int Epochs { get; set; }
            public bool Active { get; set; } = false;
            public List<Rect> Rects { get; set; } = new List<Rect>();

            public CancellationTokenSource cancellationToken;
        }
        public Data data;
        public void Bind()
        {
            Binding Slider1Bind = new()
            {
                Source = data,
                Path = new PropertyPath("A")
            };
            Slider1.SetBinding(Slider.ValueProperty, Slider1Bind);

            Binding Slider2Bind = new()
            {
                Source = data,
                Path = new PropertyPath("B")
            };
            Slider2.SetBinding(Slider.ValueProperty, Slider2Bind);

            Binding Slider3Bind = new()
            {
                Source = data,
                Path = new PropertyPath("C")
            };
            Slider3.SetBinding(Slider.ValueProperty, Slider3Bind);

        }
        public MainWindow()
        {
            InitializeComponent();
            data = new(0, 0, 0);
            Bind();
            DataContext = data;
        }

        public void Render(Individual indi)
        {
            double RightBorder = 500;
            double BottomBorder = 400;
            int INF = 1000000000;
            int w_min, h_min, w_max, h_max;
            w_min = h_min = INF;
            w_max = h_max = 0;
            for (int i = 0; i < indi.Genom.Count; ++i)
            {
                if (indi.Genom[i].width < w_min) w_min = indi.Genom[i].width;
                if (indi.Genom[i].height < h_min) h_min = indi.Genom[i].height;
                if (indi.Genom[i].width + indi.Genom[i].size > w_max) w_max = indi.Genom[i].width + indi.Genom[i].size;
                if (indi.Genom[i].height + indi.Genom[i].size > h_max) h_max = indi.Genom[i].height + indi.Genom[i].size;
            }
            double unit = Math.Min(RightBorder / (w_max - w_min + 1), BottomBorder / (h_max - h_min + 1));
            data.TotalWidth = w_max - w_min;
            data.TotalHeight = h_max - h_min;
            data.TotalSquare = data.TotalWidth * data.TotalHeight;
            data.Rects = new List<Rect>();
            foreach (Gen gen in indi.Genom)
            {
                string color;
                switch (gen.size)
                {
                    case 1:
                        color = "Green";
                        break;
                    case 2:
                        color = "Blue";
                        break;
                    case 3:
                        color = "Red";
                        break;
                    default:
                        color = "Black";
                        break;
                }
                data.Rects.Add(new((gen.width - w_min) * unit, (gen.height - h_min) * unit, gen.size * unit, gen.size * unit, color));
            }
            DataContext = null;
            DataContext = data;
        }

        public static Population Evolution(int[] nums, int size = 100, int epochs = 1000)
        {
            Population population = new(size, nums);
            for (int i = 0; i < epochs; ++i)
            {
                population.Evolve();
            }

            return population;
        }

        private async void Processing()
        {

            int[] nums = { data.A, data.B, data.C };
            if (data.A == 0 && data.B == 0 && data.C == 0) { return; }
            data.Epochs = 0;
            data.Active = true;
            Population population = new(100, nums);

            var task = Task.Factory.StartNew(() =>
            {
                while(data.Epochs < 100000)
                {
                    population.Evolve();
                    data.Epochs += 1;
                    if (data.cancellationToken.IsCancellationRequested) {  break; }
                }
            }, data.cancellationToken.Token);

            await task;
            Individual indi = population.Members[0];
            Render(indi);
        }
        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            data.cancellationToken = new CancellationTokenSource();
            Processing();
        }
        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            if (data.cancellationToken != null) data.cancellationToken.Cancel();
        }
        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            data.Active = true;
        }
    }
}



// задача вычисляет квадраты чисел


