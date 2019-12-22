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
            const int spaceCardsCount = 10007;
            var deck = Enumerable.Range(0, spaceCardsCount).ToList();


            var s = deck.Sum();
            foreach (var (technique, value) in shuffleInstructions)
            {
                switch (technique)
                {
                    case Techniques.DealIntoNewStack:
                        deck.Reverse();
                        break;
                    case Techniques.Cut:
                        if (value > 0)
                        {
                            var top = deck.Take(value);
                            var bottom = deck.Skip(value);
                            deck = bottom.Concat(top).ToList();
                        }
                        else
                        {
                            var top = deck.Take(deck.Count + value);
                            var bottom = deck.Skip(deck.Count + value);
                            deck = bottom.Concat(top).ToList();
                        }
                        break;
                    case Techniques.DealWithIncrement:
                        var updatedDeck = Enumerable.Range(0, spaceCardsCount)
                            .Select(i => int.MaxValue)
                            .ToList();

                        var index = 0;
                        foreach (var deckValue in deck)
                        {
                            updatedDeck[index] = deckValue;
                            index += value;
                            index = index < deck.Count ? index : index - deck.Count;
                        }
                        deck = updatedDeck;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var part1 = deck.IndexOf(2019);

            Console.WriteLine($"Part1 {part1}");
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

    public enum Techniques
    {
        DealIntoNewStack,
        Cut,
        DealWithIncrement
    }

}
