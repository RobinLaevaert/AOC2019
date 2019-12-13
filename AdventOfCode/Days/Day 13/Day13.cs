using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks.Dataflow;

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
            throw new NotImplementedException();
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
