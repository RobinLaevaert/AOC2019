using System;
using System.Collections.Generic;
using Day_1;
using Day_10;
using Day_11;
using Day_12;
using Day_13;
using Day_14;
using Day_2;
using Day_3;
using Day_4;
using Day_5;
using Day_6;
using Day_7;
using Day_8;
using Day_9;
using Shared;

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
                new Day5(),
                new Day6(),
                new Day7(),
                new Day8(),
                new Day9(),
                new Day10(),
                new Day11(),
                new Day12(),
                new Day13(),
                new Day14(),
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
