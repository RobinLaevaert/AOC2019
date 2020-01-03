using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Shared;

namespace Day_7
{
    public class Day7 : Day
    {

        public List<int> CodeAmpA;
        public List<int> CodeAmpB;
        public List<int> CodeAmpC;
        public List<int> CodeAmpD;
        public List<int> CodeAmpE;

        public Day7()
        {
            DayNumber = 7;
            Title = "Amplification Circuit";
        }
        public override void ReadFile()
        {
            CodeAmpA = File.ReadAllText(GetFilePath()).Split(',').Select(x => Convert.ToInt32(x)).ToList();
            CodeAmpB = File.ReadAllText(GetFilePath()).Split(',').Select(x => Convert.ToInt32(x)).ToList();
            CodeAmpC = File.ReadAllText(GetFilePath()).Split(',').Select(x => Convert.ToInt32(x)).ToList();
            CodeAmpD = File.ReadAllText(GetFilePath()).Split(',').Select(x => Convert.ToInt32(x)).ToList();
            CodeAmpE = File.ReadAllText(GetFilePath()).Split(',').Select(x => Convert.ToInt32(x)).ToList();
        }

        public override void Part1()
        {
            var phases = "01234";
            var possibleAnswers = new List<int>();
            
            foreach (var permutation in phases.Permutations())
            {
                var permPhases = permutation.Select(x => Convert.ToInt32(char.GetNumericValue(x))).ToList();
                possibleAnswers.Add(ResetAndCalcWithPhases(permPhases[0], permPhases[1], permPhases[2], permPhases[3], permPhases[4]));
            }
                
            

            var answer = possibleAnswers.Max();
            Console.WriteLine($"Answer is: {answer}");

        }

        public override async void Part2()
        {
            var code = File.ReadAllText(GetFilePath()).Split(',').Select(x => Convert.ToInt32(x));
            var outputs = new List<int>();
            var phases = "98765";

            foreach (var permutation in phases.Permutations())
            {
                var numbers = permutation.Select(x => Convert.ToInt32(char.GetNumericValue(x))).ToList();

                var bufferA = new BufferBlock<int>();
                bufferA.Post(numbers[0]);
                bufferA.Post(0);

                var bufferB = new BufferBlock<int>();
                bufferB.Post(numbers[1]);

                var bufferC = new BufferBlock<int>();
                bufferC.Post(numbers[2]);

                var bufferD = new BufferBlock<int>();
                bufferD.Post(numbers[3]);

                var bufferE = new BufferBlock<int>();
                bufferE.Post(numbers[4]);

                var bufferList = new List<BufferBlock<int>>()
                {
                    bufferA,
                    bufferB,
                    bufferC,
                    bufferD,
                    bufferE
                };

                var Amps = new List<Task>()
                {
                    ComputeV2(code.ToList(), bufferList[0], bufferList[1]),
                    ComputeV2(code.ToList(), bufferList[1], bufferList[2]),
                    ComputeV2(code.ToList(), bufferList[2], bufferList[3]),
                    ComputeV2(code.ToList(), bufferList[3], bufferList[4]),
                    ComputeV2(code.ToList(), bufferList[4], bufferList[0])
                };
                await Task.WhenAll(Amps);

                outputs.Add(bufferList[0].Receive());
            }

            Console.WriteLine($"Max output: {outputs.Max().ToString()}");
           
        }

        async Task ComputeV2(List<int> code, BufferBlock<int> inputs, BufferBlock<int> output)
        {
            var pointer = 0;
            while (pointer < code.Count && code[pointer] != 99)
            {
                var selector = code[pointer] % 10;
                var CompleteOpCode = addLeadingZeroes(code[pointer]).ToCharArray().Select(char.GetNumericValue).Select(Convert.ToInt32).ToList();

                int getParam1()
                {
                    if (CompleteOpCode[2] == 0) return code[code[pointer + 1]];
                    if (CompleteOpCode[2] == 1) return code[pointer + 1];
                    if (CompleteOpCode[2] == 2) return 7;
                    return 0;
                }
                int getParam2()
                {
                    if (CompleteOpCode[1] == 0) return code[code[pointer + 2]];
                    if (CompleteOpCode[1] == 1) return code[pointer + 2];
                    if (CompleteOpCode[1] == 2) return 7;
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
                    code[code[pointer + 1]] = await inputs.ReceiveAsync();
                    pointer += 2;
                }
                if (selector == 4)
                {
                    output.Post(getParam1());
                    pointer += 2;
                }
            }
        }
    

    private List<int> Compute(List<int> input, int phase, int password)
        {
            var running = true;
            var skip = 0;
            List<int> Output = new List<int>();
            var inputSignal = 0;
            var secret = 0 + password;
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
                    if (inputSignal == 0) secret = phase;
                    if (inputSignal == 1) secret = password;
                    input[temp.Last()] = secret;

                    inputSignal++;
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

        private int ResetAndCalcWithPhases(int a, int b, int c, int d, int e)
        {
            ReadFile();
            return Compute(CodeAmpE, e,
                Compute(CodeAmpD, d, 
                        Compute(CodeAmpC, c,
                            Compute(CodeAmpB, b, 
                                Compute(CodeAmpA, a, 
                                        0)
                                    .Last())
                                .Last())
                            .Last())
                    .Last())
                .Last();
        }
    }

    public static class EnumerableExtensions
    {
        // Source: http://stackoverflow.com/questions/774457/combination-generator-in-linq#12012418
        private static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource item)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            yield return item;

            foreach (var element in source)
                yield return element;
        }

        public static IEnumerable<IEnumerable<TSource>> Permutations<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var list = source.ToList();

            if (list.Count > 1)
                return from s in list
                    from p in Permutations(list.Take(list.IndexOf(s)).Concat(list.Skip(list.IndexOf(s) + 1)))
                    select p.Prepend(s);

            return new[] { list };
        }
    }
}
