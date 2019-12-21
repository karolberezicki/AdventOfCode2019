using IntCode;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day21
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllText("input.txt");

            var memoryState = input.Split(",")
                .Select(long.Parse)
                .ToList();

            // (!A || !B || !C) && D
            var program = new List<string>
            {
                "NOT A T", // !A => T
                "NOT B J", // !B => J
                "OR T J",  // !A || !B => J
                "NOT C T", // !C => T
                "OR T J",  // !A || !B || !C => J
                "AND D J", // (!A || !B || !C) && D => J
                "WALK",
                ""
            };

            // ((!A || !B || !C) && D) && (E || H)
            var program2 = new List<string>
            {
                "NOT A T", // !A => T
                "NOT B J", // !B => J
                "OR T J",  // !A || !B => J
                "NOT C T", // !C => T
                "OR T J",  // !A || !B || !C => J
                "AND D J", // (!A || !B || !C) && D => J
                "OR E T",  // Reset T
                "NOT T T", // Reset T
                "OR E T",  // E => T
                "OR H T",  // E|| H => T
                "AND T J", // ((!A || !B || !C) && D) && (E || H) => J
                "RUN",
                ""
            };

            var part1 = RunSpringScript(memoryState, program);
            var part2 = RunSpringScript(memoryState, program2);

            Console.WriteLine($"Part1 {part1}");
            Console.WriteLine($"Part2 {part2}");
        }

        private static long RunSpringScript(IEnumerable<long> memoryState, IEnumerable<string> program)
        {
            var icc = new IntCodeComputer(memoryState);

            foreach (var c in string.Join("\n", program))
            {
                icc.Inputs.Enqueue(c);
            }

            icc.RunTillHalt();

            while (icc.Output.Count > 1)
            {
                Console.Write((char)icc.Output.Dequeue());
            }

            return icc.Output.Dequeue();
        }
    }
}
