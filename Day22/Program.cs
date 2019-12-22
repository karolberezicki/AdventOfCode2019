using System;
using System.Collections.Generic;
using System.Linq;

namespace Day22
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllLines("input.txt")
                .ToList();

            var shuffleInstructions = ParseInput(input);
            var part1 = Shuffle(shuffleInstructions, 2019, 10007);
            Console.WriteLine($"Part1 {part1}");

            // Naive solution, that will take too long to complete :(
            var card2020Index = 2020L;
            for (var i = 0L; i <= 101741582076661; i++)
            {
                card2020Index = Shuffle(shuffleInstructions, card2020Index, 119315717514047);
            }

            var part2 = card2020Index;
            Console.WriteLine($"Part2 {part2}");
        }

        private static long Shuffle(IEnumerable<(Techniques Technique, int Value)> shuffleInstructions, long card, long cardsCount)
        {
            var cardIndex = card;

            foreach (var (technique, value) in shuffleInstructions)
            {
                switch (technique)
                {
                    case Techniques.DealIntoNewStack:
                        cardIndex = cardsCount - cardIndex - 1;
                        break;
                    case Techniques.Cut:
                        if (value > 0)
                        {
                            cardIndex = cardsCount - value + cardIndex;
                        }
                        else
                        {
                            cardIndex -= value;
                        }
                        if (cardIndex > cardsCount)
                        {
                            cardIndex -= cardsCount;
                        }
                        break;
                    case Techniques.DealWithIncrement:
                        var a = cardIndex * value;
                        var b = (long)(1.0d * value * cardIndex / cardsCount);
                        cardIndex = a - b * cardsCount;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return cardIndex;
        }

        private static List<(Techniques Technique, int Value)> ParseInput(IEnumerable<string> input)
        {
            return input.Select(i =>
            {
                if (i.Contains("cut "))
                {
                    return (Techniques.Cut, int.Parse(i.Split(" ").Last()));
                }
                if (i.Contains("deal with increment "))
                {
                    return (Techniques.DealWithIncrement, int.Parse(i.Split(" ").Last()));
                }
                return (Techniques.DealIntoNewStack, 0);
            }).ToList();
        }
    }
}
