using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Shared;

namespace Day_9
{
    public class Day9 : Day
    {

        public List<long> Code { get; set; }
        public Day9()
        {
            DayNumber = 9;
            Title = "Sensor Boost";
        }
        public override void ReadFile()
        {
            Code = File.ReadAllText(GetFilePath()).Split(',').Select(x => Convert.ToInt64(x)).ToList();
            Console.WriteLine($"What is the secret input?");
        }

        public override void Part1()
        {
            ReadFile();
            var outputBuffer  = new BufferBlock<long>();
            var inputBuffer = new BufferBlock<long>();
            var input = Convert.ToInt64(Console.ReadLine());
            inputBuffer.Post(input);
            var intCodeComputer = new IntCode(Code, inputBuffer, outputBuffer);
            intCodeComputer.Run();
            var output = new List<long>();
            while (outputBuffer.Count > 0)
            {
                output.Add(outputBuffer.Receive());
            }

            Console.WriteLine($"Answer = {output.Last()}");

        }

        public override void Part2()
        {
            Part1();
        }
    }
}
