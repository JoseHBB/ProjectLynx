namespace ProjectLynx.Core;

public class Cpu
{
    //Special Purpose Registers
    public ushort Pc { get; private set; } //Program Counter
    public ushort Sp { get; private set; } //Stack Pointer 
    
    //Main Register Set
    public RegisterPair Af { get; private set; } //Accumulator A
    public RegisterPair Bc { get; private set; } //Accumulator B
    public RegisterPair De { get; private set; } //Accumulator D
    public RegisterPair Hl { get; private set; } //Accumulator H

    private void SetZeroFlag(bool condition)
    {
        var tempAf = Af;
        if (condition)
        {
            tempAf.Low |= 1 << 7;
        }
        else
        {
            tempAf.Low = (byte)(tempAf.Low & ~(1 << 7));
        }
        Af = tempAf;
    }
    private void SetCarryFlag(bool condition)
    {
        var tempAf = Af;
        if (condition)
        {
            tempAf.Low |= 1 << 4;
        }
        else
        {
            tempAf.Low = (byte)(tempAf.Low & ~(1 << 4));
        }
        Af = tempAf;
    }
    private void SetSubtractionFlag(bool condition)
    {
        var tempAf = Af;
        if (condition)
        {
            tempAf.Low |= 1 << 6;
        }
        else
        {
            tempAf.Low = (byte)(tempAf.Low & ~(1 << 6));
        }
        Af = tempAf;
    }
    private void SetHalfCarryFlag(bool condition)
    {
        var tempAf = Af;
        if (condition)
        {
            tempAf.Low |= 1 << 5;
        }
        else
        {
            tempAf.Low = (byte)(tempAf.Low & ~(1 << 5));
        }
        Af = tempAf;
    }

    private void Add(byte value)
    {
        var tempAf = Af;
        var originalA = Af.High;
        var result = (ushort) (originalA + value);
        
        tempAf.High = (byte) result;
        Af = tempAf;
        
        SetZeroFlag(result == 0);
        
        SetSubtractionFlag(false);
        
        SetCarryFlag(result > 0x00FF);
        
        var halfCarry = ((Af.High & 0x000F) + (result & 0x000F)) > 0x00FF;
        SetHalfCarryFlag(halfCarry);
    }
    private void AddReg(byte opcode)
    {
        switch (opcode - 0x80)
        {
            case 0x0:
                Add(Bc.High);
                break;
            case 0x1:
                Add(Bc.Low);
                break;
            case 0x2:
                Add(De.High);
                break;
            case 0x3:
                Add(De.Low);
                break;
            case 0x4:
                Add(Hl.High);
                break;
            case 0x5:
                Add(Hl.Low);
                break;
            case 0x6:
                Add(Af.High);
                break;
        }
    }
    public void RunOpcode(byte opcode)
    {
        
    }
}