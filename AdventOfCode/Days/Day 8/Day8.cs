using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Shared;

namespace Day_8
{
    public class Day8 : Day
    {
        private List<int> Input;
        
        public Day8()
        {
            DayNumber = 8;
            Title = "Space Image Format";
        }
        public override void ReadFile()
        {
            Input= File.ReadAllText(GetFilePath()).ToCharArray()
                .Select(x => Convert.ToInt32(char.GetNumericValue(x))).ToList();
        }

        public override void Part1()
        {
            ReadFile();
            Console.WriteLine($"How many pixels wide ?");
            var width = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"How many pixels tall ?");
            var height = Convert.ToInt32(Console.ReadLine());

            var layerCount = Input.Count / (height * width);

            var layers = new List<List<List<int>>>();

            for (int i = 0; i < layerCount; i++)
            {
                var layer = new List<List<int>>();
                for (int j = 0; j < height; j++)
                {
                    var currentLine = new List<int>();
                    for (int k = 0; k < width; k++)
                    {
                        currentLine.Add(Input[(i * height * width) + (j * width) + k]);
                    }
                    layer.Add(currentLine);
                }
                layers.Add(layer);
            }

            var leastZeroes = layers.OrderBy(x => x.SelectMany(y => y).Count(d => d == 0)).First();
            var answer = leastZeroes.SelectMany(x => x).Count(x => x == 1) *
                         leastZeroes.SelectMany(x => x).Count(x => x == 2);
            Console.WriteLine($"The answer is: {answer}");

        }

        public override void Part2()
        {
            ReadFile();
            Console.WriteLine($"How many pixels wide ?");
            var width = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"How many pixels tall ?");
            var height = Convert.ToInt32(Console.ReadLine());
            var pixelCount = height * width;
            var layerCount = Input.Count / pixelCount;

            var layers = new List<List<List<int>>>();

            for (int i = 0; i < layerCount; i++)
            {
                var layer = new List<List<int>>();
                for (int j = 0; j < height; j++)
                {
                    var currentLine = new List<int>();
                    for (int k = 0; k < width; k++)
                    {
                        currentLine.Add(Input[(i * pixelCount) + (j * width) + k]);
                    }
                    layer.Add(currentLine);
                }
                layers.Add(layer);
            }


            var flattenedLayers = layers.Select(x => x.SelectMany(y => y).ToList()).ToList();

            var newImage = new List<List<int>>();
            for (int i = 0; i < pixelCount; i++)
            {
                var pixel = new List<int>();
                flattenedLayers.ForEach(x => pixel.Add(x[i]));
                newImage.Add(pixel);
            }

            var finalImage = newImage.Select(x => x.First(y => y != 2)).ToList();

            var Restruc = new List<List<string>>();
            for (int i = 0; i < height; i++)
            {
                var line = new List<int>();
                for (int j = 0; j < width; j++)
                {
                    line.Add(finalImage[(i * width) + j]);
                }
                Restruc.Add(line.Select(x => x.ToString()).ToList());
            }

            var reformat = Restruc.Select(x => string.Join("", x));
            foreach (var s in reformat)
            {
                foreach (var c in s.ToCharArray())
                {
                    Console.ForegroundColor = c == 48 ? ConsoleColor.Black : ConsoleColor.Green;
                    Console.Write(c == 48 ? '.' : 'X');
                }
                Console.Write(System.Environment.NewLine);
            }
            Console.ResetColor();
        }
    }
}
