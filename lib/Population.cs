using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSquares
{
    public class Population
    {
        public int size;
        public List<Individual> Members;
        public Population(int size, int[] nums)
        {
            this.size = size;
            Members = new List<Individual>();
            for (int i = 0; i < size; ++i)
            {
                Members.Add(new Individual(nums));
            }
        }

        public void Evolve(double p1 = 0.5, double p2 = 0.5)
        {
            Random randomizer = new();
            for (int i = 0; i < 2 * size; ++i)
            {
                int index = randomizer.Next(0, size);
                Individual indi = new Individual(Members[index]);
                Members.Add(indi);
            }
            for (int i = 0; i < 2 * size; ++i)
            {
                int index1 = randomizer.Next(0, size);
                int index2 = randomizer.Next(0, size);
                Individual indi = new Individual(Members[index1], Members[index2]);
                Members.Add(indi);
            }
            Members = Members.OrderBy(x => x.Fitness()).ToList().GetRange(0, size);

        }

    }
}
