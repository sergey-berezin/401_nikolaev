using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSquares
{
    public class Individual
    {
        public int[] nums { get; set; }
        public List<Gen> Genom { get; set; }
        public Individual() { }
        public int Fitness()
        {
            int INF = 1000000000;
            int w_min, h_min, w_max, h_max;
            w_min = h_min = INF;
            w_max = h_max = 0;
            for (int i = 0; i < Genom.Count; ++i)
            {
                if (Genom[i].width < w_min) w_min = Genom[i].width;
                if (Genom[i].height < h_min) h_min = Genom[i].height;
                if (Genom[i].width + Genom[i].size > w_max) w_max = Genom[i].width + Genom[i].size;
                if (Genom[i].height + Genom[i].size > h_max) h_max = Genom[i].height + Genom[i].size;
            }
            for (int i = 0; i < Genom.Count; ++i)
            {
                for (int j = 0; j < Genom.Count; ++j)
                {
                    if (i == j) continue;
                    if (((Genom[i].width + Genom[i].size > Genom[j].width) && (Genom[j].width >= Genom[i].width) ||
                        (Genom[j].width + Genom[j].size > Genom[i].width) && (Genom[i].width >= Genom[j].width)) &&
                        ((Genom[i].height + Genom[i].size > Genom[j].height) && (Genom[j].height >= Genom[i].height) ||
                        (Genom[j].height + Genom[j].size > Genom[i].height) && (Genom[i].height >= Genom[j].height))) return INF;
                }
            }
            return (w_max - w_min) * (h_max - h_min);
        }
        public Individual(int[] nums)
        {
            this.nums = nums;
            Genom = new List<Gen>();
            int min = 0;
            int max = nums[0] + nums[1] * 2 + nums[2] * 3;
            Random randomizer = new();
            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = 0; j < nums[i]; j++)
                {
                    Gen gen = new Gen(randomizer.Next(min, max), randomizer.Next(min, max), i + 1);
                    Genom.Add(gen);
                }
            }
        }

        public Individual(Individual indi)
        {
            nums = indi.nums;
            Genom = [];
            int min = 0;
            int max = nums[0] + nums[1] * 2 + nums[2] * 3;
            Random randomizer = new();
            for (int i = 0; i < indi.Genom.Count; ++i)
            {
                double x = randomizer.NextDouble();
                if (x < 0.33)
                {
                    Genom.Add(indi.Genom[i]);
                }
                else if(x < 0.66){
                    Gen gen = new(randomizer.Next(min, max), randomizer.Next(min, max), indi.Genom[i].size);
                    Genom.Add(gen);
                }
                else
                {
                    int new_width = indi.Genom[i].width + randomizer.Next(-1, 1);
                    if (0 > new_width || new_width > max)
                    {
                        new_width = indi.Genom[i].width;
                    }
                    int new_height = indi.Genom[i].height + randomizer.Next(-1, 1);
                    if (0 > new_height || new_height > max)
                    {
                        new_height = indi.Genom[i].height;
                    }
                    Gen gen = new Gen(new_width, new_height, indi.Genom[i].size);
                    Genom.Add(gen);
                }
            }
        }

        public Individual(Individual indi1, Individual indi2)
        {
            nums = indi1.nums;
            Genom = new List<Gen>();
            Random randomizer = new();
            for (int i = 0; i < indi1.Genom.Count; ++i)
            {
                double x = randomizer.NextDouble();
                if (x < 0.5)
                {
                    Genom.Add(indi1.Genom[i]);
                }
                else
                {
                    Genom.Add(indi2.Genom[i]);
                }
            }
        }
    }
}
