using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shared;

namespace Day_22
{
    public class Day22 : Day
    {
        private List<int> cards;
        private List<Shuffle> shuffles;


        public Day22()
        {
            DayNumber = 22;
            Title = "Slam Shuffle";
        }
        public override void ReadFile()
        {
            
            shuffles = File.ReadAllLines(GetFilePath()).Select(Shuffle.Create).ToList();
        }

        public override void Part1()
        {
            ReadFile();
            Console.WriteLine("How many cards do you have?");
            var cardCount = Convert.ToInt32(Console.ReadLine());
            cards = Enumerable.Range(0, cardCount).ToList();
            shuffles.ForEach(x =>
            {
                switch (x.ShuffleType)
                {
                    case ShuffleType.Cut:
                        cards = Cut(cards, x.n).ToList();
                        break;
                    case ShuffleType.IncrementDeal:
                        cards = DealWithIncrement(cards, x.n).ToList();
                        break;
                    case ShuffleType.newStack:
                        cards = NewStack(cards).ToList();
                        break;
                }
            });
            //Console.WriteLine($"Result: ");
            //Console.WriteLine(String.Join(' ', cards));
            Console.WriteLine("Card 2019 has an index of:");
            Console.WriteLine($"{cards.IndexOf(2019)}");

        }

        public override void Part2()
        {
            
                ReadFile();
                Console.WriteLine("How many cards do you have?");
                var cardCount = Convert.ToInt64(Console.ReadLine());
                Console.WriteLine("Shuffle how many times ?");
                var shuffleCount = Convert.ToInt64(Console.ReadLine());
                //var cardCount = 10;
                //var shuffleCount = 1;
                Console.WriteLine("Which Card To Track ?");
                long index = Convert.ToInt64(Console.ReadLine());

                var card = 0 + index;

                for (int j = 0; j < shuffleCount; j++)
                {
                    shuffles.ForEach(x =>
                    {
                        switch (x.ShuffleType)
                        {
                            case ShuffleType.Cut:
                                index = Cut2(index, cardCount, x.n);
                                break;
                            case ShuffleType.IncrementDeal:
                                index = DealWithIncrement2(index, cardCount, x.n);
                                break;
                            case ShuffleType.newStack:
                                index = NewStack2(index, cardCount);
                                break;
                        }
                    });
                }
                Console.WriteLine($"Index of {card}: {index}");
        }

        private IEnumerable<int> NewStack(IEnumerable<int> input)
        {
            return input.Reverse();
        }

        private IEnumerable<int> Cut(IEnumerable<int> input, int n)
        {
            var inputList = input.ToList();
            if (n < 0)
            {
                n = Math.Abs(n);
                var cardsInCut = inputList.TakeLast(n).ToList();
                inputList.RemoveRange(inputList.Count - n, n);
                cardsInCut.AddRange(inputList);
                return cardsInCut;
            }
            else
            {
                var cardsInCut = input.Take(n);
                inputList.RemoveRange(0, n);
                inputList.AddRange(cardsInCut);
                return inputList;
            }
        }

        private long Cut2(long indexCurrentCard, long cardCount, long n)
        {
            if (n < 0)
            {
                n = Math.Abs(n);
                if (indexCurrentCard >= cardCount - n)
                     return indexCurrentCard = indexCurrentCard - (cardCount - n);
                return indexCurrentCard += n;
            }
            else
            {
                if (indexCurrentCard >= n)
                    return indexCurrentCard -= n;
                return indexCurrentCard += (cardCount - n);
            }

        }

        private long NewStack2(long indexCurrentCard, long cardCount)
        {
            return cardCount - (indexCurrentCard + 1);
        }

        private long DealWithIncrement2(long indexCurrentCard, long cardCount, long n)
        {
            return (indexCurrentCard * n) % cardCount;
        }

        private IEnumerable<int> DealWithIncrement(IEnumerable<int> input, int n)
        {
            var inputList = input.ToList();
            var index = 0;
            var inputLength = inputList.Count;
            var output = new int[inputLength];

            while (inputList.Count > 0)
            {
                output[index] = inputList.First();
                inputList.RemoveRange(0,1);
                index = (index + n) % inputLength;
            }

            return output;
        }
    }

    public class Shuffle
    {
        public ShuffleType ShuffleType { get; set; }
        public int n { get; set; }

        public static Shuffle Create(string input)
        {
            ShuffleType shuffleType = ShuffleType.newStack;
            int n = 0;
            if (input.Contains("cut", StringComparison.OrdinalIgnoreCase))
            {
                shuffleType = ShuffleType.Cut;
                n = Convert.ToInt32(input.Split(" ").Last());
            }
            if (input.Contains("deal with increment", StringComparison.OrdinalIgnoreCase))
            {
                shuffleType = ShuffleType.IncrementDeal;
                n = Convert.ToInt32(input.Split(" ").Last());
            }
            if (input.Contains("deal into new stack", StringComparison.OrdinalIgnoreCase))
            {
                shuffleType = ShuffleType.newStack;
                n = 0;
            }

            return new Shuffle()
            {
                ShuffleType = shuffleType,
                n = n
            };
        }
    }

    public enum ShuffleType
    {
        newStack,
        Cut,
        IncrementDeal
    }
}
