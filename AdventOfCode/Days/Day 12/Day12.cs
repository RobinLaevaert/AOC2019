using Shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day_12
{
    public class Day12 : Day
    {
        public IEnumerable<Moon> Moons;
        public List<string> output = new List<string>();
        public Day12()
        {
            DayNumber = 12;
            Title = "The N-Body Problem";
        }
        public override void Part1()
        {
            ReadFile();
            var step = 0;
            Console.WriteLine("After 0 steps");
            Moons.ToList().ForEach(x => x.Print());
            while (step <= 999)
            {
                Console.WriteLine($"After step {step}:");
                Console.WriteLine();
                Step();
                step++;
                output.Add(Moons.ToList()[0].output());
                CalculateEnergies();
            }
            File.WriteAllLines(@"output12.txt", output);
        }

        public long indexX = 1;
        public long indexY = 1;
        public long indexZ = 1;
        ConcurrentBag<long> hitsX = new ConcurrentBag<long>();
        ConcurrentBag<long> hitsY = new ConcurrentBag<long>();
        ConcurrentBag<long> hitsZ = new ConcurrentBag<long>();
        public override void Part2()
        {
            ReadFile();
            var XCoordsAndVel = Moons.Select(x => coord.Create(x.Coordinates.X, 0)).ToList();
            var YCoordsAndVel = Moons.Select(x => coord.Create(x.Coordinates.Y, 0)).ToList();
            var ZCoordsAndVel = Moons.Select(x => coord.Create(x.Coordinates.Z, 0)).ToList();

            var found = false;
            long temp = 0;

            while (!found)
            {
                Parallel.Invoke(
                    () => Xcoords(),
                    () => Ycoords(),
                    () => ZCoords());

                if (hitsX.ToList().Any() &&
                    hitsY.ToList().Any() &&
                    hitsZ.ToList().Any())
                {
                    var answer = GetLeastCommonMultiplier(
                        GetLeastCommonMultiplier(hitsX.ToList().OrderBy(x => x).First(), hitsY.ToList().OrderBy(x => x).First()),
                        hitsZ.ToList().OrderBy(x => x).First());
                    found = true;
                    Console.WriteLine($"{answer}");
                }

            }

            void Xcoords()
            {
                var start = 0 + indexX;
                var end = 1000000 + start;
                while (indexX < end)
                {
                    XCoordsAndVel.ForEach(x => { x.UpdateVelocity(XCoordsAndVel.ToList()); });
                    XCoordsAndVel.ForEach(x => { x.ApplyVelocity(); });

                    if (XCoordsAndVel[0].X == Moons.ToList()[0].Coordinates.X &&
                        XCoordsAndVel[0].Vx == Moons.ToList()[0].Velocity.X &&
                        XCoordsAndVel[1].X == Moons.ToList()[1].Coordinates.X &&
                        XCoordsAndVel[1].Vx == Moons.ToList()[1].Velocity.X &&
                        XCoordsAndVel[2].X == Moons.ToList()[2].Coordinates.X &&
                        XCoordsAndVel[2].Vx == Moons.ToList()[2].Velocity.X &&
                        XCoordsAndVel[3].X == Moons.ToList()[3].Coordinates.X &&
                        XCoordsAndVel[3].Vx == Moons.ToList()[3].Velocity.X)
                    {
                        hitsX.Add(indexX);
                    }
                    indexX++;
                    
                }
            }
            void Ycoords()
            {
                var start = 0 + indexY;
                var end = 1000000 + start;
                while (indexY < end)
                {
                    YCoordsAndVel.ForEach(x => { x.UpdateVelocity(YCoordsAndVel.ToList()); });
                    YCoordsAndVel.ForEach(x => { x.ApplyVelocity(); });
                    if (YCoordsAndVel[0].X == Moons.ToList()[0].Coordinates.Y &&
                        YCoordsAndVel[0].Vx == Moons.ToList()[0].Velocity.Y &&
                        YCoordsAndVel[1].X == Moons.ToList()[1].Coordinates.Y &&
                        YCoordsAndVel[1].Vx == Moons.ToList()[1].Velocity.Y &&
                        YCoordsAndVel[2].X == Moons.ToList()[2].Coordinates.Y &&
                        YCoordsAndVel[2].Vx == Moons.ToList()[2].Velocity.Y &&
                        YCoordsAndVel[3].X == Moons.ToList()[3].Coordinates.Y &&
                        YCoordsAndVel[3].Vx == Moons.ToList()[3].Velocity.Y)
                    {
                        hitsY.Add(indexY);
                    }

                    indexY++;
                    
                }
            }

            void ZCoords()
            {
                var start = 0 + indexZ;
                var end = 1000000 + start;
                while (indexZ < end)
                {
                    ZCoordsAndVel.ForEach(x => { x.UpdateVelocity(ZCoordsAndVel.ToList()); });
                    ZCoordsAndVel.ForEach(x => { x.ApplyVelocity(); });
                    if (ZCoordsAndVel[0].X == Moons.ToList()[0].Coordinates.Z &&
                        ZCoordsAndVel[0].Vx == Moons.ToList()[0].Velocity.Z &&
                        ZCoordsAndVel[1].X == Moons.ToList()[1].Coordinates.Z &&
                        ZCoordsAndVel[1].Vx == Moons.ToList()[1].Velocity.Z &&
                        ZCoordsAndVel[2].X == Moons.ToList()[2].Coordinates.Z &&
                        ZCoordsAndVel[2].Vx == Moons.ToList()[2].Velocity.Z &&
                        ZCoordsAndVel[3].X == Moons.ToList()[3].Coordinates.Z &&
                        ZCoordsAndVel[3].Vx == Moons.ToList()[3].Velocity.Z)
                    {
                        hitsZ.Add(indexZ);
                    }

                    indexZ++;
                    
                }
            }
        }

        public override void ReadFile()
        {
            IEnumerable<string> temp= File.ReadAllLines(GetFilePath());
            temp = temp.Select(x => x.Replace(">", "").Replace("<", "").Replace(" ", "").Replace("y=", "").Replace("z=", "").Replace("x=", ""));
            Moons = temp.Select(x =>
            {
                var vars = x.Split(',').Select(y => Convert.ToInt32(y)).ToList();
                return Moon.Create(vars[0], vars[1], vars[2]);
            }).ToList();
        }

        public void Step()
        {
            Moons.ToList().ForEach(x => x.AdjustVelocity(Moons.ToList()));
            Moons.ToList().ForEach(x => x.ApplyVelocity());

            Moons.ToList().ForEach(x => x.Print());
        }

        public void CalculateEnergies()
        {
            Moons.ToList().ForEach(x =>
            {
                x.CalculateEnergy();
                x.PrintEnergy();
            });
            Console.WriteLine($"Sum of total energy: {Moons.ToList()[0].TotalEnergy} + {Moons.ToList()[1].TotalEnergy} + {Moons.ToList()[2].TotalEnergy} * {Moons.ToList()[3].TotalEnergy} = {Moons.Sum(x => x.TotalEnergy)}");
        }

        public long GetLeastCommonMultiplier(long a, long b)
        {
            return (a / GetGreatestCommonFactor(a, b)) * b;
        }

        static long GetGreatestCommonFactor(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public class coord
        {
            public int X;
            public int Vx;

            public void UpdateVelocity(List<coord> coords)
            {
                coords.ForEach(x =>
                {
                    if (x.X > X) Vx++;
                    if (x.X < X) Vx--;
                });
            }

            public void ApplyVelocity()
            {
                X += Vx;
            }

            public static coord Create(int pos, int velocity)
            {
                return new coord()
                {
                    X = pos,
                    Vx = velocity
                };
            }
        }
    }
}
