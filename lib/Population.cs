using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSquares
{
    public static class Utils
    {
        private static Random rng = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
    public class Population
    {
        public int size { get; set; }
        public List<Individual> Members { get; set; }
        public int[] nums { get; set; }
        public Population() { }
        public Population(int size, int[] nums)
        {
            this.size = size;
            this.nums = nums;
            Members = new List<Individual>();
            for (int i = 0; i < size; ++i)
            {
                Members.Add(new Individual(nums));
            }
        }

        public Population(Population orig)
        {
            this.nums = orig.nums;
            this.size = orig.size;
            Members = new List<Individual>(orig.Members);
        }

        public void Evolve()
        {
            Random randomizer = new();
            for (int i = 0; i < 2 * size; ++i)
            {
                int index = randomizer.Next(0, size);
                Individual indi = new(Members[index]);
                Members.Add(indi);
            }
            for (int i = 0; i < 2 * size; ++i)
            {
                int index1 = randomizer.Next(0, size);
                int index2 = randomizer.Next(0, size);
                Individual indi = new(Members[index1], Members[index2]);
                Members.Add(indi);
            }
            Utils.Shuffle(Members);
            Members = Members.OrderBy(x => x.Fitness()).ToList().GetRange(0, size);
        }

        public async void Evolve100Async()
        {
            Random randomizer = new();
            uint numPopulations = 10;

            List<Task> TaskList = [];

            for (int i = 0; i < numPopulations; ++i)
            {
                Task<Population> task = Task<Population>.Factory.StartNew(() =>
                {
                    Population TmpPopulation = new(this);

                    for (int j = 0; j < 100; ++j)
                    {
                        TmpPopulation.Evolve();
                    }
                    return TmpPopulation;
                });
                TaskList.Add(task);
            }

            Task.WaitAll([.. TaskList]);
            Members = [];
            foreach (Task<Population> task in TaskList)
            {
                Members.AddRange(task.Result.Members);
            }
            Utils.Shuffle(Members);
            Members = Members.OrderBy(x => x.Fitness()).ToList().GetRange(0, size);
        }

    }
}
