using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using Shared;

namespace Day_13
{
    public class Day13 : Day
    {
        public List<long> Input;
        public BufferBlock<long> inputBuffer;
        public BufferBlock<long> outputBuffer;
        public Day13()
        {
            DayNumber = 13;
            Title = "Care Package";
        }
        public override void Part1()
        {
            ReadFile();
            inputBuffer = new BufferBlock<long>();
            outputBuffer = new BufferBlock<long>();
            var computer = new IntCode(Input, inputBuffer, outputBuffer);
            computer.Run();
            var outputList = new List<long>();
            while(outputBuffer.Count > 0)
            {
                outputList.Add(outputBuffer.Receive());
            }
            var numberOfBlocks = outputList.Count(x => x == 2);
            var actualField = new List<drawing>();
            while(outputList.Count > 0)
            {
                var temp = outputList.Take(3).ToList();
                actualField.Add(drawing.Create(temp[0], temp[1], temp[2]));
                outputList.RemoveRange(0, 3);
            }
            Console.WriteLine($"There are {actualField.Count(x => x.Type == 2)} blocks");
        }

        public override void Part2()
        {
            
            ReadFile();
            Input[0] = 2;
            inputBuffer = new BufferBlock<long>();
            for (int i = 0; i < 1000; i++)
            {
                inputBuffer.Post(-1);
            }
            outputBuffer = new BufferBlock<long>();
            
            Console.Clear();
            var numberOfBlocksRemaining = 9999999;
            
            while (numberOfBlocksRemaining != 0)
            {
                Console.ReadKey();
                Console.Clear();

                var computer = new IntCode(Input, inputBuffer, outputBuffer);
                computer.Run();
                var outputList = new List<long>();
                while (outputBuffer.Count > 0)
                {
                    outputList.Add(outputBuffer.Receive());
                }
                var numberOfBlocks = outputList.Count(x => x == 2);
                var actualField = new List<drawing>();
                while (outputList.Count > 0)
                {
                    var temp = outputList.Take(3).ToList();
                    actualField.Add(drawing.Create(temp[0], temp[1], temp[2]));
                    outputList.RemoveRange(0, 3);
                }
                Console.WriteLine($"There are {actualField.Count(x => x.Type == 2)} blocks");
                numberOfBlocksRemaining = actualField.Count(x => x.Type == 2);

                var ballPosition = actualField.First(x => x.Type == 4);
                var PaddlePosition = actualField.First(x => x.Type == 3);
                var toMove = ballPosition.xPos - PaddlePosition.xPos;
                var toMoveWay = toMove > 0;

                inputBuffer = new BufferBlock<long>();
                for (int i = 0; i < Math.Abs(toMove); i++)
                {
                    if (toMoveWay) inputBuffer.Post(1);
                    if (!toMoveWay) inputBuffer.Post(-1);
                }
                for (int i = 0; i < 1000; i++)
                {
                    inputBuffer.Post(0);
                }
                var score = actualField.Where(x => x.xPos == -1 && x.yPos == 0);
                actualField.RemoveAll(x => x.xPos == -1 && x.yPos == 0);
                var actualscore = score.Select(x => x.Type);
                var maxWidth = actualField.Max(x => x.xPos) + 1;
                var maxHeight = actualField.Max(x => x.yPos) + 1;
                var test = new long[maxHeight, maxWidth];
                actualField.ForEach(x => test[x.yPos, x.xPos] = x.Type);

                for (int i = 0; i < test.GetLength(0); i++)
                {
                    for (int j = 0; j < test.GetLength(1); j++)
                    {
                        string toWrite = "";
                        switch (test[i, j])
                        {
                            case 0: toWrite = " "; break;
                            case 1: toWrite = "||"; break;
                            case 2: toWrite = "[]"; break;
                            case 3: toWrite = "_"; break;
                            case 4: toWrite = "O"; break;
                        }
                        Console.Write(toWrite.ToString().PadLeft(3));

                    }

                    Console.WriteLine();
                }
                
            }
            
        }

        public override void ReadFile()
        {
            Input = File.ReadAllText(GetFilePath()).Split(',').Select(x => Convert.ToInt64(x)).ToList();
        }

        public class drawing
        {
            public long xPos { get; set; }
            public long yPos { get; set; }
            public long Type { get; set; }

            public static drawing Create(long x, long y, long type)
            {
                var drawing = new drawing();
                drawing.xPos = x;
                drawing.yPos = y;
                drawing.Type = type;
                return drawing;
            }
        }
    }
}
