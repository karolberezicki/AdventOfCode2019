using System;
using System.Collections.Generic;
using System.Linq;

namespace IntCode
{
    public class IntCodeComputer
    {
        public IntCodeComputer(IEnumerable<long> initMemoryState, params long[] inputs)
        {
            _intCode = initMemoryState
                .Select((s, i) => new { s, i = Convert.ToInt64(i) })
                .ToDictionary(x => x.i, x => x.s);

            Inputs = new List<long>();
            Inputs.AddRange(inputs);
            Output = new List<long>();
        }

        public List<long> Inputs { get; set; }
        public List<long> Output { get; set; }

        private readonly Dictionary<long, long> _intCode;
        private long _instructionPointer;
        private long _inputPointer;
        private long _relativeBase;
        public long ExecutedInstructions { get; private set; }
        public bool IsHalted { get; private set; }

        public void RunTillHalt()
        {
            while (!IsHalted)
            {
                RunIntCode();
            }
        }

        public void RunIntCode()
        {
            while (!IsHalted)
            {
                ExecutedInstructions++;
                var instruction = _intCode[_instructionPointer].ToString().PadLeft(5, '0');
                var opCode = (OpCode)(GetNumber(instruction[3]) * 10 + GetNumber(instruction[4]));
                var modeParam1 = (Mode)GetNumber(instruction[2]);
                var modeParam2 = (Mode)GetNumber(instruction[1]);
                var modeParam3 = (Mode)GetNumber(instruction[0]);

                switch (opCode)
                {
                    case OpCode.Sum:
                        {
                            var param1Value = GetParamValue(modeParam1, _instructionPointer + 1);
                            var param2Value = GetParamValue(modeParam2, _instructionPointer + 2);
                            var value = param1Value + param2Value;
                            WriteMem(modeParam3, _instructionPointer + 3, value);
                            _instructionPointer += 4;
                            break;
                        }
                    case OpCode.Multiply:
                        {
                            var param1Value = GetParamValue(modeParam1, _instructionPointer + 1);
                            var param2Value = GetParamValue(modeParam2, _instructionPointer + 2);
                            var value = param1Value * param2Value;
                            WriteMem(modeParam3, _instructionPointer + 3, value);
                            _instructionPointer += 4;
                            break;
                        }
                    case OpCode.Input:
                        {
                            WriteMem(modeParam1, _instructionPointer + 1, Inputs[(int)_inputPointer]);
                            _inputPointer++;
                            _instructionPointer += 2;
                            break;
                        }
                    case OpCode.Output:
                        {
                            var param2Value = GetParamValue(modeParam1, _instructionPointer + 1);
                            Output.Add(param2Value);
                            _instructionPointer += 2;
                            return;
                        }
                    case OpCode.JumpIfTrue:
                        {
                            var param1Value = GetParamValue(modeParam1, _instructionPointer + 1);
                            var param2Value = GetParamValue(modeParam2, _instructionPointer + 2);
                            _instructionPointer = param1Value != 0 ? param2Value : _instructionPointer + 3;
                            break;
                        }
                    case OpCode.JumpIfFalse:
                        {
                            var param1Value = GetParamValue(modeParam1, _instructionPointer + 1);
                            var param2Value = GetParamValue(modeParam2, _instructionPointer + 2);
                            _instructionPointer = param1Value == 0 ? param2Value : _instructionPointer + 3;
                            break;
                        }
                    case OpCode.LessThan:
                        {
                            var param1Value = GetParamValue(modeParam1, _instructionPointer + 1);
                            var param2Value = GetParamValue(modeParam2, _instructionPointer + 2);
                            WriteMem(modeParam3, _instructionPointer + 3, param1Value < param2Value ? 1 : 0);
                            _instructionPointer += 4;
                            break;
                        }
                    case OpCode.Equals:
                        {

                            var param1Value = GetParamValue(modeParam1, _instructionPointer + 1);
                            var param2Value = GetParamValue(modeParam2, _instructionPointer + 2);
                            WriteMem(modeParam3, _instructionPointer + 3, param1Value == param2Value ? 1 : 0);
                            _instructionPointer += 4;
                            break;
                        }
                    case OpCode.RelativeBaseOffset:
                        {
                            var param1Value = GetParamValue(modeParam1, _instructionPointer + 1);
                            _relativeBase += param1Value;
                            _instructionPointer += 2;
                            break;
                        }
                    case OpCode.Halt:
                        IsHalted = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"!Unknown OpCode {opCode}!");
                }
            }
        }

        private long GetParamValue(Mode mode, long parameter)
        {

            switch (mode)
            {
                case Mode.Position:
                    {
                        return ReadMem(ReadMem(parameter));
                    }
                case Mode.Immediate:
                    {
                        return ReadMem(parameter);
                    }
                case Mode.Relative:
                    {
                        return ReadMem(ReadMem(parameter) + _relativeBase);
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException($"!Unknown parameter mode {mode}!");
                    }
            }
        }

        private long ReadMem(long address)
        {
            return _intCode.ContainsKey(address) ? _intCode[address] : 0;
        }

        private void WriteMem(Mode mode, long parameter, long value)
        {
            var address = mode == Mode.Relative ? ReadMem(parameter) + _relativeBase : ReadMem(parameter);
            _intCode[address] = value;
        }

        private static int GetNumber(char character)
        {
            return character - '0';
        }

        public void SetNoun(long noun)
        {
            _intCode[1] = noun;
        }

        public void SetVerb(long verb)
        {
            _intCode[2] = verb;
        }

        public long ReadIntCode(long address)
        {
            return ReadMem(address);
        }
    }
}