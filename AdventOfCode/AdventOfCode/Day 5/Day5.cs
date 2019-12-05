using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.Day_5
{
    class Day5 : Day
    {
        public List<int> Code;
        public List<int> Output = new List<int>();
        public int secret;

        public Day5()
        {
            dayNumber = 5;
            Title = "Sunny with a Chance of Asteroids";
        }

        public override void Part1()
        {
            ReadFile();
            Console.WriteLine($"The code is: {Compute(Code).Last()}");;
        
        }

        public override void Part2()
        {
            Part1(); // works both with same compute
        }

        public override void ReadFile()
        {
            Code = File.ReadAllText(GetFilePath()).Split(',').Select(x => Convert.ToInt32(x)).ToList();
            Console.WriteLine($"What is the secret input?");
            secret = Convert.ToInt32(Console.ReadLine());
        }

        private List<int> Compute(List<int> input)
        {
            var running = true;
            var skip = 0;

            while (running)
            {
                var temp = input.Skip(skip).ToList();
                var toTake = getToTake(temp.First());
                if (toTake == 0) break;
                
                temp = input.Skip(skip).Take(toTake).ToList();
                var CompleteOpCode = addLeadingZeroes(temp.First()).ToCharArray().Select(x => char.GetNumericValue(x)).ToList();

                var selector = CompleteOpCode.Last();

                if (selector == 1 || selector == 2 || selector == 5 || selector == 6 || selector == 7 || selector == 8)
                {
                    
                    int param1;
                    int param2;
                    param1 = CompleteOpCode[2] == 0 ? input[temp[1]] : temp[1];
                    param2 = CompleteOpCode[1] == 0 ? input[temp[2]] : temp[2];
                    if (selector == 1 || selector == 2) input[temp[3]] = selector == 1 ? param1 + param2 : param1 * param2;
                    if( selector == 5 && param1 != 0)
                    {
                        skip = 0;
                        toTake = param2;
                    }
                    if (selector == 6 && param1 == 0)
                    {
                        skip = 0;
                        toTake = param2;
                    }

                    if(selector == 7)
                    {
                        input[temp[3]] = param1 < param2 ?  1 : 0;
                    }
                    if (selector == 8)
                    {
                        input[temp[3]] = param1 == param2 ? 1 : 0;
                    }
                }
                if (selector == 3)
                {
                    input[temp.Last()] = secret;
                }
                if (selector == 4)
                {
                    var param1 = CompleteOpCode[2] == 0 ? input[temp[1]] : temp[1];
                    Output.Add(param1);
                }
                
                
                skip += toTake;
            }
            return Output;
        }

        private int getToTake(int number)
        {
            if (number == 1 || number == 2 || number == 7 || number == 8) return 4;
            if (number == 3 || number == 4) return 2;
            if (number == 5 || number == 6) return 3;
            if (number == 99) return 0;
            
            return getToTake(number % 10);
        }

        private string addLeadingZeroes(int input)
        {
            return input.ToString("D5");
        }
    }
}
