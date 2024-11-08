using GeneticSquares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class Data(int a, int b, int c)
    {
        public int A { get; set; } = a;
        public int B { get; set; } = b;
        public int C { get; set; } = c;

        public int TotalWidth { get; set; }
        public int TotalHeight { get; set; }
        public int TotalSquare { get; set; }
        public int Epochs { get; set; }
        public List<Rect> Rects { get; set; } = new List<Rect>();

        public CancellationTokenSource? cancellationToken;
        public Population BestPopulation { get; set; } = new(10000, [a, b, c]);
    }
}
