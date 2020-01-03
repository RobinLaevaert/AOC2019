using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shared;

namespace Day_16
{
    public class Day16 : Day
    {
        private readonly List<int> _basePattern = new List<int>(){0,1,0,-1};
        private List<int> code;

        public Day16()
        {
            DayNumber = 16;
            Title = "Flawed Frequency Transmission";
        }
        public override void ReadFile()
        {
            code = File.ReadAllText(GetFilePath()).ToIntList();
        }

        public override void Part1()
        {
            var phases = 100;
            ReadFile();
            var test = code;
            for (int i = 0; i < phases; i++)
            {
                var resulttest = new List<int>();
                for (int j = 0; j < test.Count; j++)
                {
                    resulttest.Add(CalculateSegment(j, test));
                }

                test = resulttest;
            }
            
        }

        public override void Part2()
        {
            var toSkip = 4;
            var toTake = 3;
            var phases = 1;
            ReadFile();
            var test = "12345678".ToIntList();
            for (int i = 0; i < phases; i++)
            {
                var resulttest = new List<int>();
                for (int j = toSkip; j < (toSkip+ toTake); j++)
                {
                    resulttest.Add(CalculateSegmentV2(j, test, toSkip));
                }

                test = resulttest;
            }
        }

        public int GetFactor(int phase, List<int> pattern, int pos)
        {
            phase++;
            var list = new List<int>();
            for (int i = 0; i < pattern.Count; i++)
            {
                for (int j = 0; j < phase; j++)
                {
                    list.Add(pattern[i]);
                }
            }

            
            return list[(pos+1)%list.Count];
        }

        public int CalculateSegment(int segment, List<int> input)
        {
            
            int[] testarray = input.ToArray();
            var temp = new List<int>();
            for (int i = 0; i < testarray.Length; i++)
            {
                temp.Add(GetFactor(segment, _basePattern, i));
            }

            var tempresult = 0;
            for (int i = 0; i < testarray.Length; i++)
            {
                tempresult += (testarray[i] * temp[i]);
            }

            return Math.Abs(tempresult) % 10;
        }

        public int CalculateSegmentV2(int segment, List<int> input, int skipped)
        {

            int[] testarray = input.ToArray();
            var temp = new List<int>();
            for (int i = 0; i < testarray.Length; i++)
            {
                temp.Add(GetFactor(segment, _basePattern, i + skipped));
            }

            var tempresult = 0;
            for (int i = 0; i < testarray.Length; i++)
            {
                tempresult += (testarray[i] * temp[i]);
            }

            return Math.Abs(tempresult) % 10;
        }
    }

    public static class listExtentions
    {
        public static List<int> ToIntList(this string input)
        {
            return input.ToCharArray().Select(x => Convert.ToInt32(char.GetNumericValue(x))).ToList();
        }
    }
}
