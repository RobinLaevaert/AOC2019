using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Shared;

namespace Day_3
{
    public class Day3 : Day
    {
        List<PathInstruction> Wire1;
        List<PathInstruction> Wire2;

        public Day3()
        {
            DayNumber = 3;
            Title = "Crossed Wires";
        }
        public override void Part1()
        {
            ReadFile();
            var WirePath1 = GetCoordinates(Wire1);
            var WirePath2 = GetCoordinates(Wire2);
            var intersections = WirePath1.Intersect(WirePath2, new YourEqualityComparer());
            var distance = intersections.Select(x => new
            {
                coordinates = x,
                distance = Math.Abs(x.x) + Math.Abs(x.y)
            }).OrderBy(x => x.distance).Skip(1).ToList();
            var closest = distance.First();
            Console.WriteLine($"Closes point is: ({closest.coordinates.x}.{closest.coordinates.y}) with a distance of {closest.distance}");
        }

        public override void Part2()
        {
            ReadFile();
            var WirePath1 = GetCoordinates(Wire1);
            var WirePath2 = GetCoordinates(Wire2);
            var intersections = WirePath1.Intersect(WirePath2, new YourEqualityComparer());

            var steps = intersections.Select(x =>
            {
                 return new coord
                {
                    x = x.x,
                    y = x.y,
                    stepsTaken = WirePath1.First(y => y.Equals(x)).stepsTaken +
                                 WirePath2.First(y => y.Equals(x)).stepsTaken
                };
            }).OrderBy(x => x.stepsTaken).Skip(1);

            
            Console.WriteLine($"Fewest combined steps is: {steps.First().stepsTaken}");
        }

        public override void ReadFile()
        {
            var temp = File.ReadAllLines(GetFilePath());
            Wire1 = temp[0].Split(',').Select(x => new PathInstruction
            {
                Direction = x[0],
                Distance = Convert.ToInt32(x.Substring(1))
            }).ToList();
            Wire2 = temp[1].Split(',').Select(x => new PathInstruction
            {
                Direction = x[0],
                Distance = Convert.ToInt32(x.Substring(1))
            }).ToList();
        }
        public List<coord> GetCoordinates(List<PathInstruction> input)
        {
            var x = 0;
            var y = 0;
            var tempx = 0;
            var tempy = 0;
            var steps = 0;
            var output = new List<coord>();
            input.ForEach(instruction =>
            {
                switch (instruction.Direction)
                {
                    case 'R':
                        tempx += instruction.Distance;
                        break;
                    case 'L':
                        tempx -= instruction.Distance;
                        break;
                    case 'U':
                        tempy += instruction.Distance;
                        break;
                    case 'D':
                        tempy -= instruction.Distance;
                        break;
                }

                while (tempx != x || tempy != y)
                {
                    Add();
                    if (tempx > x) x++;
                    if (tempx < x) x--;
                    if (tempy > y) y++;
                    if (tempy < y) y--;
                }

            });
            return output;

            void Add()
            {
                output.Add(new coord()
                {
                    x = x,
                    y = y,
                    stepsTaken = steps
                });
                steps++;
            }
        }
    }


    public class PathInstruction
    {
        public char Direction { get; set; }
        public int Distance { get; set; }
    }

    public class coord : IEquatable<coord>
    {
        public int x { get; set; }
        public int y { get; set; }

        public int stepsTaken { get; set; }
        public bool Equals([AllowNull] coord other)
        {
            return this.x == other.x && this.y == other.y;
        }
    }

    public class YourEqualityComparer : IEqualityComparer<coord>
    {

        #region IEqualityComparer<ThisClass> Members


        public bool Equals(coord x, coord y)
        {
            //no null check here, you might want to do that, or correct that to compare just one part of your object
            return x.x == y.x && x.y == y.y;
        }


        public int GetHashCode(coord obj)
        {
            unchecked
            {
                var hash = 17;
                //same here, if you only want to get a hashcode on a, remove the line with b
                hash = hash * 23 + obj.x.GetHashCode();
                hash = hash * 23 + obj.y.GetHashCode();

                return hash;
            }
        }

        #endregion
    }
}
