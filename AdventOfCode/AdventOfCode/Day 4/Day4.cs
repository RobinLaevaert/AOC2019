using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Day_4
{
    class Day4 : Day
    {
        int lowerLimit;
        int upperLimit;
        List<int> range = new List<int>();
        public Day4()
        {
            dayNumber = 4;
            Title = "Secure Container";
        }

        public override void Part1()
        {
            ReadFile();
            var answer = range.Where(x => HasAdjacentDigits(x) && DoesNotDecrease(x)).Distinct();
            System.Console.WriteLine($"The number of passwords is: {answer.Count()}");
        }

        public override void Part2()
        {
            ReadFile();
            var answer = range.Where(x => CheckForPart2(x) && DoesNotDecrease(x)).ToList();
            System.Console.WriteLine($"The number of passwords is: {answer.Count()}");
        }

        public override void ReadFile()
        {
            System.Console.WriteLine($"What is the range?");
            System.Console.WriteLine();
            System.Console.Write($"Lower Limit: ");
            lowerLimit = Convert.ToInt32(Console.ReadLine());
            System.Console.Write($"Upper Limit: ");
            upperLimit = Convert.ToInt32(Console.ReadLine());
            range = Enumerable.Range(lowerLimit, upperLimit-lowerLimit).ToList();
        }

        public bool HasAdjacentDigits(int input){
            
            var prev = input % 10;
            input /= 10;
            
            while (input > 0)
                {
                    var current = input % 10;
                    if (current == prev)
                        return true;
                    prev = current;
                    input /= 10;
                }
            return false;

        }

        public bool CheckForPart2(int input)
        {
            var freshinput = 0 + input;
            var prev = input % 10;
            input /= 10;
            var counter = new int[10]
            {
                0,0,0,0,0,0,0,0,0,0
            };

            while (input > 0)
            {
                var current = input % 10;
                if (current == prev)
                    counter[current]++;
                prev = current;
                input /= 10;
            }
            if (counter.Any(x => x == 1))
            {
                return true;
            }
            return false;
        }

        public bool DoesNotDecrease(int input){
            var intlist = input.ToString().Select(digit => int.Parse(digit.ToString())).ToList();
            return intlist[0] <= intlist[1] && intlist[1] <= intlist[2] && intlist[2] <= intlist[3] && intlist[3] <= intlist[4] && intlist[4] <= intlist[5];
        }
    }
}
