using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.Day_6
{
    public class Day6 : Day
    {
        List<CustomOrbit> input;
        public Day6()
        {
            Title = "Universal Orbit Map";
            dayNumber = 6;
        }
        public override void Part1()
        {
            ReadFile();
            var distances = input.Select(x => GetStepsToCom(x)).ToList();
            var answer = distances.Sum();
            System.Console.WriteLine($"Answer = {answer}");
        }

        public override void Part2()
        {
            ReadFile();
            CustomOrbit startpath1 = input.First(x => x.orbital == "YOU");
            CustomOrbit startpath2 = input.First(x => x.orbital == "SAN");
            var yourPath = GetPath(startpath1);
            var santaPath = GetPath(startpath2);
            var intersectionPoint = yourPath.Intersect(santaPath, new YourEqualityComparer()).ToList().First();
            var yourPathSteps = yourPath.GetRange(0, yourPath.IndexOf(intersectionPoint)).Count();
            var santaPathSteps = santaPath.GetRange(0, santaPath.IndexOf(intersectionPoint)).Count();
            var answer = yourPathSteps + santaPathSteps - 2; // The steps to COM need to be ignored
            System.Console.WriteLine($"The answer is: {answer}");
        }

        public override void ReadFile()
        {
            input = File.ReadAllLines(GetFilePath()).Select(x => new CustomOrbit(x)).ToList();
        }

        public int GetStepsToCom(CustomOrbit orbit){
            if(orbit.center == "COM") return 1; // this is COM
            return 1 + GetStepsToCom(input.First(x => x.orbital == orbit.center));
        }

        public List<CustomOrbit> GetPath(CustomOrbit orbit){
            var path = new List<CustomOrbit>();
            var notCenter = true;
            path.Add(orbit);
            while(notCenter){
                if(orbit.center == "COM"){
                    notCenter = true;
                    break;
                }
                orbit = input.First(x => x.orbital == orbit.center);
                path.Add(orbit);
            }
            return path;
        }
    }

    public class CustomOrbit{
        public string center { get; set; }
        public string orbital { get; set; }

    public CustomOrbit(string input){
        var split = input.Split(')');
        center = split.First();;
        orbital = split.Last();
    }

    


    }

     public class YourEqualityComparer : IEqualityComparer<CustomOrbit>
    {

        #region IEqualityComparer<ThisClass> Members


        public bool Equals(CustomOrbit x, CustomOrbit y)
        {
            //no null check here, you might want to do that, or correct that to compare just one part of your object
            return x.orbital == y.orbital && x.center == y.center;
        }


        public int GetHashCode(CustomOrbit obj)
        {
            unchecked
            {
                var hash = 17;
                //same here, if you only want to get a hashcode on a, remove the line with b
                hash = hash * 23 + obj.center.GetHashCode();
                hash = hash * 23 + obj.orbital.GetHashCode();

                return hash;
            }
        }

        #endregion
    }
}
