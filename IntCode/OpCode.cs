namespace IntCode
{
    public enum OpCode : long
    {
        Sum = 1L,
        Multiply,
        Input,
        Output,
        JumpIfTrue,
        JumpIfFalse,
        LessThan,
        Equals,
        RelativeBaseOffset,
        Halt = 99L
    }
}