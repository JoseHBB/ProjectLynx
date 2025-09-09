namespace ProjectLynx.Core;

public struct RegisterPair
{
    public ushort Word;

    public byte High
    {
        get => (byte)(Word >> 8);
        set => Word = (ushort)(value << 8 | (Word & 0xFF00));
    }

    public byte Low
    {
        get => (byte)(Word & 0x00FF);
        set => Word = (ushort)((Word & 0xFF00) | value);
    }
}