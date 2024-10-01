using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSquares
{
    public struct Gen(int width, int height, int size)
    {
        public int width = width;
        public int height = height;
        public int size = size;
    }
}
