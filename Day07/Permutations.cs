using System.Collections.Generic;
using System.Linq;

namespace Day07
{
    public static class Permutations
    {
        public static List<List<T>> GeneratePermutations<T>(List<T> items)
        {
            var currentPermutation = new T[items.Count];
            var inSelection = new bool[items.Count];
            var results = new List<List<T>>();
            PermuteItems(items, inSelection, currentPermutation, results, 0);
            return results;
        }

        private static void PermuteItems<T>(IReadOnlyList<T> items, IList<bool> inSelection,
            IList<T> currentPermutation, ICollection<List<T>> results,
            int nextPosition)
        {

            if (nextPosition == items.Count)
            {
                results.Add(currentPermutation.ToList());
            }
            else
            {
                for (var i = 0; i < items.Count; i++)
                {
                    if (inSelection[i])
                    {
                        continue;
                    }

                    inSelection[i] = true;
                    currentPermutation[nextPosition] = items[i];
                    PermuteItems(items, inSelection, currentPermutation, results, nextPosition + 1);
                    inSelection[i] = false;
                }
            }
        }
    }
}