using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shared;


namespace Day_1
{
    public class Day1 : Day
    {
        public List<double> input { get; set; }
        public Day1()
        {
            DayNumber = 1;
            Title = "The Tyranny of the Rocket Equation";
        }

        public override void Part1()
        {
            ReadFile();
            input = input.Select(x => getFuelCost(x, 1)).ToList();
            Console.WriteLine(input.Sum());
        }

        public override void Part2()
        {
            ReadFile();
            input = input.Select(x => getFuelCost(x, 2)).ToList();
            Console.WriteLine(input.Sum());
        }

        public override void ReadFile()
        {
            input = File.ReadAllLines(GetFilePath()).Select(Convert.ToDouble).ToList();
        }

        private double getFuelCost(double input, int part)
        {
            var temp = Math.Floor(input / 3) - 2;
            if (part == 1) return temp;
            if (temp <= 0) return 0;
            else return temp + getFuelCost(temp, 2);
        }
    }
}
