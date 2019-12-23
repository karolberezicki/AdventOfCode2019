using IntCode;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day23
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllText("input.txt");

            var memoryState = input.Split(",")
                .Select(long.Parse)
                .ToList();


            var part1 = Part1(memoryState);
            var part2 = Part2(memoryState);

            Console.WriteLine($"Part1 {part1}");
            Console.WriteLine($"Part2 {part2}");
        }

        private static long Part1(IReadOnlyCollection<long> memoryState)
        {
            var network = new Dictionary<long, IntCodeComputer>();
            for (var networkAddress = 0; networkAddress < 50; networkAddress++)
            {
                network.Add(networkAddress, new IntCodeComputer(memoryState));
                network[networkAddress].Inputs.Enqueue(networkAddress);
            }

            while (true)
            {
                foreach (var (_, icc) in network)
                {
                    if (!icc.Inputs.Any())
                    {
                        icc.Inputs.Enqueue(-1);
                    }

                    icc.RunIntCode(BreakMode.Input);

                    var outputs = icc.Output.ToList();
                    icc.Output.Clear();

                    for (var i = 0; i < outputs.Count; i += 3)
                    {
                        var address = outputs[i];
                        var x = outputs[i + 1];
                        var y = outputs[i + 2];

                        if (address == 255)
                        {
                            return y;
                        }

                        network[address].Inputs.Enqueue(x);
                        network[address].Inputs.Enqueue(y);
                    }
                }
            }
        }

        private static long Part2(IReadOnlyCollection<long> memoryState)
        {
            var network = new Dictionary<long, IntCodeComputer>();
            for (var networkAddress = 0; networkAddress < 50; networkAddress++)
            {
                network.Add(networkAddress, new IntCodeComputer(memoryState));
                network[networkAddress].Inputs.Enqueue(networkAddress);
            }

            var nat = (X: 0L, Y: 0L);
            var natHistory = new HashSet<long>();
            while (true)
            {
                foreach (var (key, icc) in network)
                {
                    if (key == 0)
                    {
                        var allEmptyIncoming = network
                            .Select(n => n.Value.Inputs)
                            .SelectMany(i => i).All(v => v == -1);

                        var allOutputsEmpty = network
                            .Select(n => n.Value.Output.Count)
                            .All(o => o == 0);

                        var isNetworkIdle = allEmptyIncoming && allOutputsEmpty;

                        if (isNetworkIdle)
                        {
                            if (natHistory.Contains(nat.Y))
                            {
                                return nat.Y;
                            }
                            icc.Inputs.Enqueue(nat.X);
                            icc.Inputs.Enqueue(nat.Y);
                            natHistory.Add(nat.Y);
                        }
                    }

                    if (!icc.Inputs.Any())
                    {
                        icc.Inputs.Enqueue(-1);
                    }

                    icc.RunIntCode(BreakMode.Input);

                    var outputs = icc.Output.ToList();
                    icc.Output.Clear();

                    for (var i = 0; i < outputs.Count; i += 3)
                    {
                        var address = outputs[i];
                        var x = outputs[i + 1];
                        var y = outputs[i + 2];

                        if (address == 255)
                        {
                            nat = (x, y);
                        }
                        else
                        {
                            network[address].Inputs.Enqueue(x);
                            network[address].Inputs.Enqueue(y);
                        }
                    }
                }
            }
        }
    }
}
