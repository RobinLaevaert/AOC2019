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
            throw new NotImplementedException();
        }

        public override void ReadFile()
        {
            input = File.ReadAllLines(GetFilePath()).Select(x => new CustomOrbit(x)).ToList();
        }

        public int GetStepsToCom(CustomOrbit orbit){
            if(orbit.center == "COM") return 1; // this is COM
            return 1 + GetStepsToCom(input.First(x => x.orbital == orbit.center));
        }
    }

    public class CustomOrbit{
        public string center { get; set; }
        public string orbital { get; set; }

    public CustomOrbit(string input){
        if(input.Contains("COM")){

        }
        var split = input.Split(')');
        center = split.First();;
        orbital = split.Last();
            
    }
    }
}
