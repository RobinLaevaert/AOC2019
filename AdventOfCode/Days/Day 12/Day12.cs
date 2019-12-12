using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            while (step <= 9999)
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

        public override void Part2()
        {
            throw new NotImplementedException();
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
            //Console.WriteLine($"Sum of total energy: {Moons.ToList()[0].TotalEnergy} + {Moons.ToList()[1].TotalEnergy} + {Moons.ToList()[2].TotalEnergy} * {Moons.ToList()[3].TotalEnergy} = {Moons.Sum(x => x.TotalEnergy)}");


        }
        public class Moon
        {
            private Moon()
            {
                Velocity = new Velocity();
                Coordinates = new Coord();
            }
            public Velocity Velocity { get; set; }

            public Coord Coordinates { get; set; }
            public int KineticEnergy { get; set; }
            public int PotentialEnergy { get; set; }
            public int TotalEnergy { get; set; }

            public void ApplyVelocity()
            {
                Coordinates.ApplyVelocity(Velocity);
            }

            public void AdjustVelocity(List<Moon> otherMoons)
            {
                otherMoons.ForEach(otherMoon =>
                {
                    if (Coordinates.X != otherMoon.Coordinates.X)
                    {
                        if (otherMoon.Coordinates.X > Coordinates.X) Velocity.X++;
                        if (otherMoon.Coordinates.X < Coordinates.X) Velocity.X--;
                    }
                    if (Coordinates.Y != otherMoon.Coordinates.Y)
                    {
                        if (otherMoon.Coordinates.Y > Coordinates.Y) Velocity.Y++;
                        if (otherMoon.Coordinates.Y < Coordinates.Y) Velocity.Y--;
                    }
                    if (Coordinates.Z != otherMoon.Coordinates.Z)
                    {
                        if (otherMoon.Coordinates.Z > Coordinates.Z) Velocity.Z++;
                        if (otherMoon.Coordinates.Z < Coordinates.Z) Velocity.Z--;
                    }
                });
            }

            public void Step(List<Moon> otherMoons)
            {
                otherMoons = otherMoons.Where(x => x != this).ToList();
                AdjustVelocity(otherMoons);
                ApplyVelocity();
            }

            public static Moon Create(int x, int y, int z)
            {
                var moon = new Moon();
                moon.Coordinates.Set(x, y, z);
                return moon;
            }

            public void CalculateEnergy()
            {
                PotentialEnergy = Math.Abs(Coordinates.X) + Math.Abs(Coordinates.Y) + Math.Abs(Coordinates.Z);
                KineticEnergy = Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z);
                TotalEnergy = PotentialEnergy * KineticEnergy;
            }

            public void Print()
            {
                Console.WriteLine($"pos=<x= {Coordinates.X.ToString().PadLeft(3)}, y=  {Coordinates.Y.ToString().PadLeft(3)}, z= {Coordinates.Z.ToString().PadLeft(3)}>, vel=<x= {Velocity.X.ToString().PadLeft(3)}, y= {Velocity.Y.ToString().PadLeft(3)}, z= {Velocity.Z.ToString().PadLeft(3)}>");
            }

            public void PrintEnergy()
            {
                Console.WriteLine($"Pot: {Coordinates.X.ToString().PadLeft(3)} + {Coordinates.Y.ToString().PadLeft(3)} + {Coordinates.Z.ToString().PadLeft(3)} = {PotentialEnergy.ToString().PadLeft(3)}; Kin: {Velocity.X.ToString().PadLeft(3)} + {Velocity.Y.ToString().PadLeft(3)} + {Velocity.Z.ToString().PadLeft(3)} = {KineticEnergy.ToString().PadLeft(3)}; Total: {PotentialEnergy.ToString().PadLeft(3)} + {KineticEnergy.ToString().PadLeft(3)} = {TotalEnergy.ToString().PadLeft(3)}");
            }

            public string output()
            {
                return $"{Coordinates.X},{Coordinates.Y},{Coordinates.Z},{Velocity.X},{Velocity.Y},{Velocity.Z},{KineticEnergy},{PotentialEnergy},{TotalEnergy}";
            }
        }

        public class Coord
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }

            public void ApplyVelocity(Velocity velocity)
            {
                X += velocity.X;
                Y += velocity.Y;
                Z += velocity.Z;
            }

            public void Set(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

        public class Velocity
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
        }
    }
}
