using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shared;

namespace Day_24
{
    public class Day24 : Day
    {
        public List<Tile> tiles = new List<Tile>();
        public Day24()
        {
            DayNumber = 24;
            Title = "Planet of Discord";
        }
        public override void ReadFile()
        {
            var test = File.ReadAllLines(GetFilePath()).Select(x => x.ToCharArray()).ToArray();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                   
                    tiles.Add(Tile.Create(test[j][i], i, j));
                }
            }
        }

        public override void Part1()
        {
            var biodiversityList = new List<double>();
            ReadFile();
            //printField();
            while (biodiversityList.Count == biodiversityList.Distinct().Count())
            {
                biodiversityList.Add(CalculateBiodiversity(tiles));
                tiles.ForEach(t => t.CalculateNextStep(tiles));
                tiles.ForEach(t => t.update());
                //Console.WriteLine();
                //printField();
            }
            printField();
            var hash = new HashSet<double>();
            var duplicates = biodiversityList.Where(i => !hash.Add(i));
            Console.WriteLine();
            Console.WriteLine($"{duplicates.First()}");

        }

        public override void Part2()
        {
            throw new NotImplementedException();
        }

        private void printField()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < 5; j++)
                {
                    var toPrint = tiles.First(x => x.x == j && x.y == i).HasBugs ? '#' : '.';
                    Console.Write($"{toPrint}");
                }
            }
        }
        public double CalculateBiodiversity(List<Tile> tiles)
        {
            double total = 0;
            var test = tiles.Where(x => x.HasBugs).ToList();
            test.ForEach(x => { total += Math.Pow(2, ((x.y * 5) + x.x)); });
            return total;
        }
    }

    public class Tile
    {
        public bool HasBugs;
        public int x;
        public int y;
        public bool NextStep;

        public static Tile Create(char input, int x, int y)
        {
            var newTile = new Tile
            {
                HasBugs = false || input == '#',
                x = x,
                y = y
            };
            return newTile;
        }

        public void update()
        {
            HasBugs = NextStep;
        }

        public void CalculateNextStep(List<Tile> tiles)
        {
            var adjacentTiles = tiles.Where(t =>
                (t.y == y && (t.x == x + 1 || t.x == x - 1)) || (t.x == x && (t.y == y + 1 || t.y == y - 1)));
            if (HasBugs)
            {
                NextStep = adjacentTiles.Count(t => t.HasBugs) == 1;
            }
            else
            {
                NextStep = adjacentTiles.Count(t => t.HasBugs) == 1 || adjacentTiles.Count(t => t.HasBugs) == 2; ;
            }
        }
    }
}
