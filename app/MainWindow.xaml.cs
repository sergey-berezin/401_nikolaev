using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
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

using GeneticSquares;
using static GeneticApplication.MainWindow;



namespace GeneticApplication
{
    public partial class MainWindow : Window
    {        
        public Data data = new(0, 0, 0);
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

        private async void Processing()
        {
            while (data.Epochs < 100000)
            {
                Task task = Task.Factory.StartNew((o) =>
                {
                    do
                    {
                        data.BestPopulation.Evolve100Async();
                        data.Epochs += 100;
                        if (data.cancellationToken.IsCancellationRequested) { break; }
                    } while (data.Epochs % 100 != 0);

                }, TaskCreationOptions.LongRunning, data.cancellationToken.Token);

                try
                {
                    await task;
                }
                catch (Exception ex) 
                {
                    Individual i = data.BestPopulation.Members[0];
                    Render(i);
                    return;
                }
                Individual indi = data.BestPopulation.Members[0];
                Render(indi);
            }
        }
        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            int[] nums = { data.A, data.B, data.C };
            if (data.A == 0 && data.B == 0 && data.C == 0) 
            {
                return;
            } else if (nums.SequenceEqual(data.BestPopulation.nums))
            {
                BStart.IsEnabled = false;
                BStop.IsEnabled = true;
                data.cancellationToken = new CancellationTokenSource();
                Processing();
            } else
            {
                BStart.IsEnabled = false;
                BStop.IsEnabled = true;
                data.Epochs = 0;
                data.BestPopulation = new(100, nums);
                data.cancellationToken = new CancellationTokenSource();
                Processing();
            }

        }
        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            BStart.IsEnabled = true;
            BStop.IsEnabled = false;
            if (data.cancellationToken != null) data.cancellationToken.Cancel();
        }
    }
}