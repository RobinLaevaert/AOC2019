using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Shared;

namespace Day_5
{
    public class Day5 : Day
    {
        public List<int> Code;
        

        public Day5()
        {
            DayNumber = 5;
            Title = "Sunny with a Chance of Asteroids";
        }

        public override async void Part1()
        {
            ReadFile();
            var test = Convert.ToInt32(Console.ReadLine());
            BufferBlock<int> testblock = new BufferBlock<int>();
            await Task.WhenAll(ComputeV2(Code, test, testblock));
            Console.WriteLine($"The code is: {Compute(Code, test).Last()}"); ;
            var test2 = new List<int>();
            while (testblock.Count > 0)
            {
                test2.Add(testblock.Receive());
            }
        }

        

        public override void Part2()
        {
            Part1(); // works both with same compute
        }

        public override void ReadFile()
        {
            Code = File.ReadAllText(GetFilePath()).Split(',').Select(x => Convert.ToInt32(x)).ToList();
            Console.WriteLine($"What is the secret input?");
        }

        private List<int> Compute(List<int> input, int secret)
        {
            var running = true;
            var skip = 0;
            List<int> Output = new List<int>();
            while (running)
            {
                var temp = input.Skip(skip).ToList();
                var toTake = getToTake(temp.First());
                if (toTake == 0) break;

                temp = input.Skip(skip).Take(toTake).ToList();
                var CompleteOpCode = addLeadingZeroes(temp.First()).ToCharArray().Select(char.GetNumericValue).Select(Convert.ToInt32).ToList();

                var selector = CompleteOpCode.Last();

                if (selector == 1 || selector == 2 || selector == 5 || selector == 6 || selector == 7 || selector == 8)
                {
                    var param1 = CompleteOpCode[2] == 0 ? input[temp[1]] : temp[1];
                    var param2 = CompleteOpCode[1] == 0 ? input[temp[2]] : temp[2];
                    if (selector == 1 || selector == 2) input[temp[3]] = selector == 1 ? param1 + param2 : param1 * param2;
                    if (selector == 5 && param1 != 0)
                    {
                        skip = 0;
                        toTake = param2;
                    }
                    if (selector == 6 && param1 == 0)
                    {
                        skip = 0;
                        toTake = param2;
                    }

                    if (selector == 7)
                    {
                        input[temp[3]] = param1 < param2 ? 1 : 0;
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


        async Task ComputeV2(List<int> code, int password, BufferBlock<int> output)
        {
            var pointer = 0;
            var relativeBase = 0;

            while (pointer < code.Count && code[pointer] != 99)
            {


                var selector = code[pointer] % 10;
                var CompleteOpCode = addLeadingZeroes(code[pointer]).ToCharArray().Select(char.GetNumericValue).Select(Convert.ToInt32).ToList();

                int getParam1()
                {
                    if (CompleteOpCode[2] == 0) return code[code[pointer + 1]];
                    if (CompleteOpCode[2] == 1) return code[pointer + 1];
                    if (CompleteOpCode[2] == 2) return code[(relativeBase + code[pointer + 1])];
                    return 0;
                }
                int getParam2()
                {
                    if (CompleteOpCode[1] == 0) return code[code[pointer + 2]];
                    if (CompleteOpCode[1] == 1) return code[pointer + 2];
                    if (CompleteOpCode[1] == 2) return code[(relativeBase + code[pointer + 1])];
                    return 0;
                }

                if (selector == 1 || selector == 2 || selector == 5 || selector == 6 || selector == 7 || selector == 8)
                {

                    var param1 = getParam1();
                    var param2 = getParam2();
                    if (selector == 1 || selector == 2)
                    {
                        code[code[pointer + 3]] = selector == 1 ? param1 + param2 : param1 * param2;
                        pointer += 4;
                    }
                    if (selector == 5)
                    {
                        pointer = param1 == 0 ? pointer + 3 : param2;
                    }
                    if (selector == 6 && param1 == 0)
                    {
                        pointer = param1 != 0 ? pointer + 3 : param2;
                    }

                    if (selector == 7)
                    {
                        code[code[pointer + 3]] = param1 < param2 ? 1 : 0;
                        pointer += 4;
                    }
                    if (selector == 8)
                    {
                        code[code[pointer + 3]] = param1 == param2 ? 1 : 0;
                        pointer += 4;
                    }
                }
                if (selector == 3)
                {
                    code[code[pointer + 1]] = password;
                    pointer += 2;
                }
                if (selector == 4)
                {
                    output.Post(getParam1());
                    pointer += 2;
                }

                if (selector == 9)
                {
                    relativeBase += getParam1();
                }
            }
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
