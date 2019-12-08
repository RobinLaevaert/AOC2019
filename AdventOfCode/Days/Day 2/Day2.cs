using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shared;

namespace Day_2
{
    public class Day2: Day
    {
        public List<int> Code;

        public Day2()
        {
            DayNumber = 2;
            Title = "1202 Program Alarm";
        }
        public override void Part1()
        {
            ReadFile();
            Code[1] = 12;
            Code[2] = 2;
            var result = Compute(Code);
            Console.WriteLine($"Answer: {result.First()}");
            Console.WriteLine("Written to file aswell");
        }

        public override void Part2()
        {
            ReadFile();
            // Brute force leggo
            for (int i = 0; i < 99; i++)
            {
                for (int j = 0; j < 99; j++)
                {
                    var set = new List<int>(Code)
                    {
                        [1] = i,
                        [2] = j
                    };
                    Compute(set);
                    if (set.First() == 19690720)
                    {
                        Console.WriteLine(i);
                        Console.WriteLine(j);
                    }
                }
            }
            Console.ReadLine();
        }

        public override void ReadFile()
        {
            Code = File.ReadAllText(GetFilePath()).Split(',').Select(x => Convert.ToInt32(x)).ToList();
        }

        private List<int> Compute(List<int> input)
        {
            var running = true;
            int loop = 0;

            while (running)
            {
                var temp = input.Skip(loop * 4).Take(4).ToList();
                switch (temp.First())
                {
                    case 1:
                        input[temp[3]] = input[temp[1]] + input[temp[2]];
                        break;
                    case 2:
                        input[temp[3]] = input[temp[1]] * input[temp[2]];
                        break;
                    case 99:
                        running = false;
                        break;
                }
                loop++;
            }
            return input;
        }
    }
}
