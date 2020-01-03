using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Shared;

namespace Day_10
{
    public class Day10 : Day
    {
        public List<List<int>> map { get; set; } //0 = empty; 1 = asteroid
        public Day10()
        {
            DayNumber = 10;
            Title = "Monitoring Station";
        }
        public override void ReadFile()
        {
            map = File.ReadAllLines(GetFilePath()).Select(x => x.ToCharArray().Select(y =>
            {
                if (y == '#') return 1;
                return 0;
            }).ToList()).ToList();

        }

        // Vertical lines !!
        public override void Part1()
        {
            ReadFile();

            var asteroids = new List<Point>();
            for (int i = 0; i < map.Count(); i++)
            {
                for (int j = 0; j < map.First().Count(); j++)
                {
                    if(map[j][i] == 1)asteroids.Add(new Point(i,j));
                }
            }

            asteroids = asteroids.OrderBy(x => x.Y).ToList();

            var currentHighest = 0;
            var bestPoint = new Point();
            
            asteroids.ForEach(originPoint =>
            {
                var angles = new List<double>();
                asteroids.ForEach(asteroid =>
                {
                    if (!asteroid.Equals(originPoint))
                    {
                        var angle = ComputeAngle(originPoint, asteroid);
                        angles.Add(angle);
                    }
                });

               
                var test = angles.Distinct().Count();
                if (test> currentHighest)
                {
                    
                    currentHighest = test;
                    bestPoint = originPoint;
                }
            });

            Console.WriteLine($"Best place would be at ({bestPoint.X},{bestPoint.Y}) with #{currentHighest} asteroids in sight");

        }

        // start at -90 go to 180 (-> -180)
        public override void Part2()
        {
            ReadFile();
            var asteroids = new List<Point>();
            for (var i = 0; i < map.Count(); i++)
            for (var j = 0; j < map.First().Count(); j++)
                if (map[j][i] == 1)
                    asteroids.Add(new Point(i, j));

            asteroids = asteroids.OrderBy(x => x.Y).ToList();

            var currentHighest = 0;
            var originPoint = new Point(26, 29);

            var angles = new List<double>();
            var temp = new List<placeholder>();
            asteroids.ForEach(asteroid =>
            {
                if (!asteroid.Equals(originPoint))
                {
                    var asteroidangle = ComputeAngle(originPoint, asteroid);
                    temp.Add(new placeholder
                    {
                        Angle = asteroidangle,
                        coord = asteroid
                    });
                    angles.Add(asteroidangle);
                }
            });
            var test = angles.OrderBy(x => x);
            var lasered = 0;
            var lastFired = -90;
            double angle = -90;
            var laseredasts = new List<double>();
            while (lasered < 200)
            {
                double toBeLasered = 0;
                try
                {
                    toBeLasered = test.First(x => x >= angle);
                }
                catch
                {
                    continue;
                }

                angles.Remove(toBeLasered);
                laseredasts.Add(toBeLasered);
                lasered++;
                if (toBeLasered == 180 || angle == 180) angle = -180;
                else angle = toBeLasered + 0.00001;
            }

            var distinctLaseredAngles = laseredasts.Distinct();
            var countOfThatAngle = distinctLaseredAngles.Count(x => x == distinctLaseredAngles.Last());

            var tempfiltered = temp.Where(x => x.Angle == distinctLaseredAngles.Last()).ToList()[1 - countOfThatAngle];

            Console.WriteLine($"The asteroid at coordinates ({tempfiltered.coord.X},{tempfiltered.coord.Y}) would be the {lasered}-th victim");

        }

        internal class placeholder
        {
            public double Angle { get; set; }
            public Point coord { get; set; }
        }

        public double ComputeAngle(Point origin, Point other)
        {
            float xDiff = other.X - origin.X;
            float yDiff = other.Y - origin.Y;
            return Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;
        }
    }
}
