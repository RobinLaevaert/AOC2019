using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shared;

namespace Day_6
{
    public class Day6 : Day
    {
        private List<CustomOrbit> input;

        public Day6()
        {
            Title = "Universal Orbit Map";
            DayNumber = 6;
        }

        public override void Part1()
        {
            ReadFile();
            var distances = input.Select(GetStepsToCom).ToList();
            var answer = distances.Sum();
            Console.WriteLine($"Answer = {answer}");
        }

        public override void Part2()
        {
            ReadFile();
            var yourPath = GetPath(input.First(x => x.orbital == "YOU"));
            var santaPath = GetPath(input.First(x => x.orbital == "SAN"));
            var intersectionPoint = yourPath.Intersect(santaPath, new YourEqualityComparer()).ToList().First();
            var yourPathSteps = yourPath.GetRange(0, yourPath.IndexOf(intersectionPoint)).Count();
            var santaPathSteps = santaPath.GetRange(0, santaPath.IndexOf(intersectionPoint)).Count();
            var answer = yourPathSteps + santaPathSteps - 2; // The steps to COM need to be ignored
            Console.WriteLine($"The answer is: {answer}");
        }

        public override void ReadFile()
        {
            input = File.ReadAllLines(GetFilePath()).Select(x => new CustomOrbit(x)).ToList();
        }

        public int GetStepsToCom(CustomOrbit orbit)
        {
            if (orbit.center == "COM") return 1; // this is COM
            return 1 + GetStepsToCom(input.First(x => x.orbital == orbit.center));
        }

        public List<CustomOrbit> GetPath(CustomOrbit orbit)
        {
            var path = new List<CustomOrbit>();
            path.Add(orbit);
            while (true)
            {
                if (orbit.center == "COM")
                {
                    break;
                }

                orbit = input.First(x => x.orbital == orbit.center);
                path.Add(orbit);
            }

            return path;
        }
    }

    public class CustomOrbit
    {
        public CustomOrbit(string input)
        {
            var split = input.Split(')');
            center = split.First();
            orbital = split.Last();
        }

        public string center { get; set; }
        public string orbital { get; set; }
    }

    public class YourEqualityComparer : IEqualityComparer<CustomOrbit>
    {
        public bool Equals(CustomOrbit x, CustomOrbit y)
        {
            return x.orbital == y.orbital && x.center == y.center;
        }


        public int GetHashCode(CustomOrbit obj)
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + obj.center.GetHashCode();
                hash = hash * 23 + obj.orbital.GetHashCode();

                return hash;
            }
        }
    }
}
