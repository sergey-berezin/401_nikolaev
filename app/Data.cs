using GeneticSquares;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GeneticApplication
{
    public class Rect(double x, double y, double w, double h, string color)
    {
        public double X { get; set; } = x;
        public double Y { get; set; } = y;
        public double Width { get; set; } = w;
        public double Height { get; set; } = h;
        public string Color { get; set; } = color;
    }
    public class Data: INotifyPropertyChanged
    {
        public int a;
        public int b;
        public int c;
        public int A
        {
            get {  return a; }
            set
            {
                a = value;
                OnPropertyChanged();
            }
        }
        public int B
        {
            get { return b; }
            set
            {
                b = value;
                OnPropertyChanged();
            }
        }
        public int C
        {
            get { return c; }
            set
            {
                c = value;
                OnPropertyChanged();
            }
        }

        public int TotalWidth { get; set; }
        public int TotalHeight { get; set; }
        public int TotalSquare { get; set; }
        public int Epochs { get; set; }
        public List<Rect> Rects { get; set; } = new List<Rect>();

        public CancellationTokenSource? cancellationToken;
        public Population BestPopulation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
