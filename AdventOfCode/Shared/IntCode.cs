using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace Shared
{
    public class IntCode
    {
        public BufferBlock<long> InputBufferBlock;
        public BufferBlock<long> OutputBufferBlock;

        public List<long> Memory;
        public int Pointer = 0;
        public int RelativeBase;

        public bool running = true;

        public IntCode(List<long> initialMemory, BufferBlock<long> inputBuffer, BufferBlock<long> outputBuffer)
        {

            InputBufferBlock = inputBuffer;
            OutputBufferBlock = outputBuffer;
            Memory = new List<long>(initialMemory);
        }

        public List<long> Run()
        {
            var output = new List<long>();
            while (running)
            {
                int opcode = (int)(Get(Pointer) % 100);
                switch (opcode)
                {
                    case 1: Addition(); break;
                    case 2: Multiplication(); break;
                    case 3: Input(); break;
                    case 4: Output(); break;
                    case 5: Jump(true); break;
                    case 6: Jump(false); break;
                    case 7: LessThan(); break;
                    case 8: Equals(); break;
                    case 9: SetRelativeBase(); break;
                    case 99: running = false; break;
                }
            }
            return output;
        }

        private void Addition()
        {
            SetRelative(3, GetRelative(1) + GetRelative(2));
            Pointer += 4;
        }

        private void Multiplication()
        {
            SetRelative(3, GetRelative(1) * GetRelative(2));
            Pointer += 4;
        }

        private void Input()
        {
            SetRelative(1, InputBufferBlock.Receive());
            Pointer += 2;
        }

        private void Output()
        {
            OutputBufferBlock.Post(GetRelative(1));
            Pointer += 2;
        }

        private void Jump(bool when)
        {
            if (when)
            {
                if (GetRelative(1) == 0)
                    Pointer += 3;
                else
                    Pointer = (int)GetRelative(2);
            }
            else
            {

                if (GetRelative(1) == 0)
                    Pointer = (int)GetRelative(2);
                else
                    Pointer += 3;
            }
            
        }

        private void LessThan()
        {
            if (GetRelative(1) < GetRelative(2))
                SetRelative(3, 1);
            else
                SetRelative(3, 0);
            Pointer += 4;
        }

        private void Equals()
        {
            if (GetRelative(1) == GetRelative(2))
                SetRelative(3, 1);
            else
                SetRelative(3, 0);
            Pointer += 4;
        }

        private void SetRelativeBase()
        {
            RelativeBase += (int)GetRelative(1);
            Pointer += 2;
        }

        private string addLeadingZeroes(long input)
        {
            return input.ToString("D5");
        }

        private long Get(int address)
        {
            if (address < Memory.Count)
                return Memory[address];
            else
            {
                return 0;
            }
        }

        private void Set(long address, long value)
        {
            while (address >= Memory.Count)
                Memory.Add(0);
            Memory[(int)address] = value;

        }

        private int accessMode(int relativePos)
        {
            var CompleteOpCode = addLeadingZeroes((int)Get(Pointer));
            return Convert.ToInt32(char.GetNumericValue(CompleteOpCode[3 - relativePos]));
        }

        public void SetRelative(int relativePos, long value)
        {
            long param = Get(Pointer + relativePos);
            switch (accessMode(relativePos))
            {
                case 0:
                    Set(param, value);
                    break;
                case 1:
                    // THIS SHOULD NOT BE POSSIBLE (Hope they didn't lie and somehow enter here \._./)
                    break;
                case 2:
                    Set(RelativeBase + param, value);
                    break;

            }
        }

        public long GetRelative(int relativePos)
        {
            long param = Get(Pointer + relativePos);
            switch (accessMode(relativePos))
            {
                case 0:
                    return Get((int)param);
                case 1:
                    return param;
                case 2:
                    return Get((int)(RelativeBase + param));
                default:
                    return 0; // should never get here anyway
            }
        }

    }
}
