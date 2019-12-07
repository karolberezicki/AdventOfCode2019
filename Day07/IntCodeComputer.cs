using System.Collections.Generic;
using System.Linq;

namespace Day07
{
    public class IntCodeComputer
    {
        public IntCodeComputer(IEnumerable<int> initMemoryState)
        {
            _intCode = initMemoryState.ToList();
            Inputs = new List<int>();
            Output = new List<int>();
        }

        public List<int> Inputs { get; set; }
        public List<int> Output { get; set; }

        private readonly List<int> _intCode;
        private int _instructionPointer;
        private int _inputPointer;
        public bool IsHalted { get; private set; }

        public void RunIntCode()
        {
            while (!IsHalted)
            {
                var instruction = _intCode[_instructionPointer].ToString().PadLeft(5, '0');
                var opCode = GetNumber(instruction[3]) * 10 + GetNumber(instruction[4]);
                var modeParam1 = GetNumber(instruction[2]);
                var modeParam2 = GetNumber(instruction[1]);

                switch (opCode)
                {
                    case 1: // sum
                    {
                        var resultAddress = _intCode[_instructionPointer + 3];
                        var param1 = modeParam1 == 0 ? _intCode[_intCode[_instructionPointer + 1]] : _intCode[_instructionPointer + 1];
                        var param2 = modeParam2 == 0 ? _intCode[_intCode[_instructionPointer + 2]] : _intCode[_instructionPointer + 2];
                        _intCode[resultAddress] = param1 + param2;
                        _instructionPointer += 4;
                        break;
                    }
                    case 2: // multiply
                    {
                        var resultAddress = _intCode[_instructionPointer + 3];
                        var param1 = modeParam1 == 0 ? _intCode[_intCode[_instructionPointer + 1]] : _intCode[_instructionPointer + 1];
                        var param2 = modeParam2 == 0 ? _intCode[_intCode[_instructionPointer + 2]] : _intCode[_instructionPointer + 2];
                        _intCode[resultAddress] = param1 * param2;
                        _instructionPointer += 4;
                        break;
                    }
                    case 3: // input
                    {
                        var address = _intCode[_instructionPointer + 1];
                        _intCode[address] = Inputs[_inputPointer];
                        _inputPointer++;
                        _instructionPointer += 2;
                        break;
                    }
                    case 4: // output
                    {
                        var param1 = modeParam1 == 0 ? _intCode[_intCode[_instructionPointer + 1]] : _intCode[_instructionPointer + 1];
                        Output.Add(param1);
                        _instructionPointer += 2;
                        return;
                    }
                    case 5: // jump-if-true
                    {
                        var param1 = modeParam1 == 0 ? _intCode[_intCode[_instructionPointer + 1]] : _intCode[_instructionPointer + 1];
                        var param2 = modeParam2 == 0 ? _intCode[_intCode[_instructionPointer + 2]] : _intCode[_instructionPointer + 2];
                        _instructionPointer = param1 != 0 ? param2 : _instructionPointer + 3;
                        break;
                    }
                    case 6: // jump-if-false
                    {
                        var param1 = modeParam1 == 0 ? _intCode[_intCode[_instructionPointer + 1]] : _intCode[_instructionPointer + 1];
                        var param2 = modeParam2 == 0 ? _intCode[_intCode[_instructionPointer + 2]] : _intCode[_instructionPointer + 2];
                        _instructionPointer = param1 == 0 ? param2 : _instructionPointer + 3;
                        break;
                    }
                    case 7: // less than
                    {
                        var param1 = modeParam1 == 0 ? _intCode[_intCode[_instructionPointer + 1]] : _intCode[_instructionPointer + 1];
                        var param2 = modeParam2 == 0 ? _intCode[_intCode[_instructionPointer + 2]] : _intCode[_instructionPointer + 2];
                        var resultAddress = _intCode[_instructionPointer + 3];
                        _intCode[resultAddress] = param1 < param2 ? 1 : 0;
                        _instructionPointer += 4;
                        break;
                    }
                    case 8: // equals
                    {

                        var param1 = modeParam1 == 0 ? _intCode[_intCode[_instructionPointer + 1]] : _intCode[_instructionPointer + 1];
                        var param2 = modeParam2 == 0 ? _intCode[_intCode[_instructionPointer + 2]] : _intCode[_instructionPointer + 2];
                        var resultAddress = _intCode[_instructionPointer + 3];
                        _intCode[resultAddress] = param1 == param2 ? 1 : 0;
                        _instructionPointer += 4;
                        break;
                    }
                    case 99: // halt
                        IsHalted = true;
                        break;
                }
            }
        }

        private static int GetNumber(char character)
        {
            return character - '0';
        }
    }
}