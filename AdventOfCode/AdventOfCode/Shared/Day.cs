using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace AdventOfCode.Shared
{
    public abstract class Day
    {
        public string Title;
        public int dayNumber;

        public Day()
        {
        }
        public void printInfo()
        {
            Console.WriteLine($"{dayNumber}. {Title}");
        }

        public void HandleSelect()
        {
            Console.WriteLine("Do you want to solve Part 1 or 2?");
            
           switch (Convert.ToDouble(Console.ReadLine())){
                case 1:
                    Part1();
                    break;
                case 2:
                    Part2();
                    break;
                default:
                    Console.WriteLine($"Not implemented");
                    HandleSelect();
                    break;
            }
        }

        public string GetFilePath()
        {
            Console.WriteLine($"What is the path for input file?");
            return Console.ReadLine();
        }
        public abstract void ReadFile();

        public abstract void Part1();

        public abstract void Part2();
    }
}
