namespace ProjectLynx.Core;

public class Cpu(Mmu.Mmu mmu)
{
    private Mmu.Mmu _mmu = mmu;
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
    public void RunOpcode(byte opcode)
    {
        
    }
    
    public void NoOp() => Pc++;

    public void HlInc()
    {
        var tempHl = Hl;
        tempHl.Word++;
        Hl = tempHl;
    }
    
    public void HlDec()
    {
        var tempHl = Hl;
        tempHl.Word--;
        Hl = tempHl;
    }
    
    public void LoadRegister(ref byte registerWrite, ref byte registerRead) //LD r8,r8
    {
        registerWrite = registerRead;
        Pc++;
    }
    
    public void LoadImmediate8(ref byte register) //LD r8,n8
    {
        register = _mmu.GetByteFromAddress((ushort)(Pc + 1));
        Pc += 2;
    }
    
    public void LoadImmediate16(ref RegisterPair registerPair) //LD r16,n16
    {
        registerPair.Low =  _mmu.GetByteFromAddress((ushort)(Pc + 1));
        registerPair.High =  _mmu.GetByteFromAddress((ushort)(Pc + 2));
        Pc += 3;
    }
    
    public void WriteMemoryLocation(ref byte register) //LD [HL],r8
    {
        _mmu.WriteByteToAddress(Hl.Word, register);
        Pc += 2;
    }

    public void WriteImmediateMemoryLocation() //LD [HL],n8
    {
        var value = _mmu.GetByteFromAddress((ushort)(Pc + 1));
        _mmu.WriteByteToAddress(Hl.Word, value);
        Pc += 3;
    }
    
    public void LoadMemoryLocation(ref byte register) //LD r8,[HL]
    {
        register = _mmu.GetByteFromAddress(Hl.Word);
        Pc += 2;
    }
    
    public void WriteMemoryLocationA(ref RegisterPair registerPair) //LD [r16],A
    {
        _mmu.WriteByteToAddress(registerPair.Word, Af.High);
        Pc += 2;
    }
    
    public void WriteImmediateMemoryLocationA() //LD [n16],A
    {
        var tempRegisterPair = new  RegisterPair
        {
            Low = _mmu.GetByteFromAddress(Pc += 1),
            High = _mmu.GetByteFromAddress(Pc += 2)
        };
        
        _mmu.WriteByteToAddress(tempRegisterPair.Word, Af.High);
        
        Pc += 3;
    }
    
    public void WriteImmediateHighMemoryLocationA() //LDH [n16],A
    {
        var byteAddress = _mmu.GetByteFromAddress((ushort)(Pc + 1));

        var finalAddress = (ushort)(byteAddress + 0xFF00);
        
        _mmu.WriteByteToAddress(finalAddress, Af.High);
        
        Pc += 2;
    }

    public void WriteHighMemoryLocationA() //LDH [C],A
    {
        _mmu.WriteByteToAddress((ushort)(0xFF00 + Bc.Low), Af.High);
        
        Pc += 2;
    }
    
    public void LoadMemoryLocationA(ref RegisterPair registerPair) //LD A,[r16]
    {
        var tempAf = Af;
        tempAf.High = _mmu.GetByteFromAddress(registerPair.Word);
        Af = tempAf;
        Pc += 2;
    }

    public void LoadImmediateMemoryLocationA() //LD A,[n16]
    {
        var tempRegister = new RegisterPair
        {
            Low = _mmu.GetByteFromAddress((ushort)(Pc + 1)),
            High = _mmu.GetByteFromAddress((ushort)(Pc + 2))
        };

        var tempAf = Af;
        tempAf.High = _mmu.GetByteFromAddress(tempRegister.Word);
        
        Af = tempAf;
        Pc += 4;
    }

    public void LoadImmediateHighMemoryLocationA() //LDH A,[n16]
    {
        var byteAddress = _mmu.GetByteFromAddress((ushort)(Pc + 1));
        
        var finalAddress = (ushort)(byteAddress + 0xFF00);
        
        var tempAf = Af;
        
        tempAf.High = _mmu.GetByteFromAddress(finalAddress);
        
        Af = tempAf;

        Pc += 3;
    }

    public void LoadHighMemoryLocationA() //LDH A,[C]
    {
        var tempAf = Af;
        
        tempAf.High = _mmu.GetByteFromAddress((ushort)(0xFF00 + Bc.Low));
        
        Af = tempAf;
        
        Pc += 2;
    }
    
    public void WriteMemoryLocationAInc() //LD [HLI],A
    {
        _mmu.WriteByteToAddress(Hl.Word, Af.High);

        Pc += 2;
        HlInc();
    }
    
    public void WriteMemoryLocationADec() //LD [HLD],A
    {
        _mmu.WriteByteToAddress(Hl.Word, Af.High);

        Pc += 2;
        HlDec();
    }
    public void LoadMemoryLocationAInc() //LD A,[HLI]
    {
        var value = _mmu.GetByteFromAddress(Hl.Word);
        
        var tempAf = Af;
        
        tempAf.High = value;
        
        Af = tempAf;

        Pc += 2;
        HlInc();
    }
    
    public void LoadMemoryLocationADec() //LD A,[HLD]
    {
        var value = _mmu.GetByteFromAddress(Hl.Word);
        
        var tempAf = Af;
        
        tempAf.High = value;
        
        Af = tempAf;

        Pc += 2;
        HlDec();
    }

    public void LoadImmediateStackPointer() //LD SP,n16
    {
        var tempRegisterPair = new RegisterPair
        {
            Low = _mmu.GetByteFromAddress((ushort)(Pc + 1)),
            High = _mmu.GetByteFromAddress((ushort)(Pc + 2))
        };

        Sp = tempRegisterPair.Word;
        
        Pc += 3;
    }

    public void WriteMemoryLocationStackPointer() //LD [n16],SP
    {
        var tempRegisterPair = new RegisterPair
        {
            Low = _mmu.GetByteFromAddress((ushort)(Pc + 1)),
            High = _mmu.GetByteFromAddress((ushort)(Pc + 2))
        };

        _mmu.WriteByteToAddress(tempRegisterPair.Word, (byte)(Sp & 0xFF));
        _mmu.WriteByteToAddress((ushort)(tempRegisterPair.Word + 1), (byte)(Sp >> 8));

        Pc += 3;
    }

    public void LoadHlStackPointerPlusE()
    {
        var offset = (sbyte)_mmu.GetByteFromAddress((ushort)(Pc + 1));

        var tempHl = Hl;

        tempHl.Word = (ushort)(tempHl.Word + offset);
        
        Hl = tempHl;
        
        SetZeroFlag(false);
        SetSubtractionFlag(false);
        
        var halfCarry = ((Sp & 0x000F) + (offset & 0x000F)) > 0x000F;
        SetHalfCarryFlag(halfCarry);
        
        SetCarryFlag(((Sp & 0x00FF) + (offset & 0x00FF)) > 0x00FF);
        
        Pc += 2;
    }
    
    private void Add(byte value)
    {
        var tempAf = Af;
        var originalA = Af.High;
        var result = (ushort) (originalA + value);
        
        tempAf.High = (byte) result;
        Af = tempAf;
        
        SetZeroFlag((byte)result == 0);
        
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
}