using AdventOfCode.Day_1;
using AdventOfCode.Day_2;
using AdventOfCode.Day_3;
using AdventOfCode.Day_4;
using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AdventOfCode
{
    class Program
    {
        public static List<Day> days = new List<Day>()
            {
                new Day1(),
                new Day2(),
                new Day3(),
                new Day4(),
                //new Day5(),
                //new Day6(),
                //new Day7(),
                //new Day8(),
                //new Day9(),
                //new Day10(),
                //new Day11(),
                //new Day12(),
                //new Day13(),
                //new Day14(),
                //new Day15(),
                //new Day16(),
                //new Day17(),
                //new Day18(),
                //new Day19(),
                //new Day20(),
                //new Day21(),
                //new Day22(),
                //new Day23(),
                //new Day24(),
                //new Day25(),

            };
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Which Day do you want ?");
                days.ForEach(x => x.PrintInfo());
                var chosenDay = Convert.ToInt32(Console.ReadLine()) - 1;
                days[chosenDay].HandleSelect();
                days[chosenDay].Deselect();
            }
        }
    }

}
