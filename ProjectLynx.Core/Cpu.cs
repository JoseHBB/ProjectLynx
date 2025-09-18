using System.Security.Principal;
using ProjectLynx.Memory;

namespace ProjectLynx.Core;

public class Cpu(Mmu mmu)
{
    private Mmu _mmu = mmu;
    //Special Purpose Registers
    public ushort Pc { get; private set; } //Program Counter
    public ushort Sp { get; private set; } //Stack Pointer 
    
    //Main Register Set
    private RegisterPair _af;
    private RegisterPair _bc;
    private RegisterPair _de;
    private RegisterPair _hl;
    public RegisterPair Af { get => _af; private set => _af = value; } //Accumulator A
    public RegisterPair Bc { get => _bc; private set => _bc = value;  } //Accumulator B
    public RegisterPair De { get => _de; private set => _de = value;  } //Accumulator D
    public RegisterPair Hl { get => _hl; private set => _hl = value;  } //Accumulator H

    private void PrintCpuData()
    {
        Console.WriteLine($"Pc: {Pc:X4}");
        Console.WriteLine($"Sp: {Sp:X4}");
        Console.WriteLine($"Af: {_af.Word:X4}");
        Console.WriteLine($"Bc: {_bc.Word:X4}");
        Console.WriteLine($"De: {_de.Word:X4}");
        Console.WriteLine($"Hl: {_hl.Word:X4}");
    }
    public void Start()
    {
        byte opcode;
        while (true)
        {
            opcode = _mmu.GetByteFromAddress(Pc);
            
            RunOpcode(opcode);
            PrintCpuData();
            
            //var keyInfo = Console.ReadKey(true);
            
            //while (keyInfo.Key != ConsoleKey.Spacebar)
            //{
            //}
        }
    }
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
        switch (opcode)
        {
            case 0x00:
                NoOperation();
                break;
            case 0x01:
                LoadImmediate16(ref _bc);
                break;
            case 0x02:
                WriteMemoryLocationA(ref _bc);
                break;
            case 0x06:
                LoadImmediate8(ref _bc.Low);
                break;
            case 0x0A:
                LoadMemoryLocationA(ref _bc);
                break;
            case 0x11:
                LoadImmediate16(ref _de);
                break;
            case 0x12:
                WriteMemoryLocationA(ref _de);
                break;
            case 0x16:
                LoadImmediate8(ref _de.Low);
                break;
            case 0x1A:
                LoadMemoryLocationA(ref _de);
                break;
            case 0x21:
                LoadImmediate16(ref _hl);
                break;
            case 0x22:
                WriteMemoryLocationAInc();
                break;
            case 0x26:
                LoadImmediate8(ref _hl.Low);
                break;
            case 0x2A:
                LoadMemoryLocationAInc();
                break;
            case 0x31:
                WriteMemoryLocationADec();
                break;
            case 0x36:
                WriteImmediateMemoryLocation();
                break;
            case 0x3A:
                LoadMemoryLocationADec();
                break;
            case 0x40:
                LoadRegister(ref _bc.High, ref _bc.High);
                break;
            case 0x41:
                LoadRegister(ref _bc.High, ref _bc.Low);
                break;
            case 0x42:
                LoadRegister(ref _bc.High, ref _de.High);
                break;
            case 0x43:
                LoadRegister(ref _bc.High, ref _de.Low);
                break;
            case 0x44:
                LoadRegister(ref _bc.High, ref _hl.High);
                break;
            case 0x45:
                LoadRegister(ref _bc.High, ref _hl.Low);
                break;
            case 0x46:
                LoadMemoryLocation(ref _bc.High);
                break;
            case 0x47:
                LoadRegister(ref _bc.High, ref _af.High);
                break;
            case 0x48:
                LoadRegister(ref _bc.Low, ref _bc.High);
                break;
            case 0x49:
                LoadRegister(ref _bc.Low, ref _bc.Low);
                break;
            case 0x4A:
                LoadRegister(ref _bc.Low, ref _de.High);
                break;
            case 0x4B:
                LoadRegister(ref _bc.Low, ref _de.Low);
                break;
            case 0x4C:
                LoadRegister(ref _bc.Low, ref _hl.High);
                break;
            case 0x4D:
                LoadRegister(ref _bc.Low, ref _hl.Low);
                break;
            case 0x50:
                LoadRegister(ref _de.High, ref _bc.High);
                break;
            case 0x51:
                LoadRegister(ref _de.High, ref _bc.Low);
                break;
            case 0x52:
                LoadRegister(ref _de.High, ref _de.High);
                break;
            case 0x53:
                LoadRegister(ref _de.High, ref  _de.Low);
                break;
            case 0x54:
                LoadRegister(ref _de.High, ref  _hl.High);
                break;
            case 0x55:
                LoadRegister(ref _de.High, ref  _hl.Low);
                break;
            case 0x56:
                LoadMemoryLocation(ref _de.High);
                break;
            case 0x57:
                LoadRegister(ref _de.High, ref _af.Low);
                break;
            case 0x58:
                LoadRegister(ref _de.Low, ref _bc.High);
                break;
            case 0x59:
                LoadRegister(ref _de.Low, ref _bc.Low);
                break;
            case 0x5A:
                LoadRegister(ref _de.Low, ref _de.High);
                break;
            case 0x5B:
                LoadRegister(ref _de.Low, ref _de.Low);
                break;
            case 0x5C:
                LoadRegister(ref _de.Low, ref _hl.High);
                break;
            case 0x5D:
                LoadRegister(ref _de.Low, ref _hl.Low);
                break;
            case 0x60:
                LoadRegister(ref _hl.High, ref _bc.High);
                break;
            case 0x61:
                LoadRegister(ref _hl.High, ref _bc.Low);
                break;
            case 0x62:
                LoadRegister(ref _hl.High, ref _de.High);
                break;
            case 0x63:
                LoadRegister(ref _hl.High, ref _de.Low);
                break;
            case 0x64:
                LoadRegister(ref _hl.High, ref _hl.High);
                break;
            case 0x65:
                LoadRegister(ref _hl.High, ref _hl.Low);
                break;
            case 0x66:
                LoadMemoryLocation(ref _hl.High);
                break;
            case 0x67:
                LoadRegister(ref _hl.High, ref _af.High);
                break;
            case 0x68:
                LoadRegister(ref _hl.Low, ref _bc.High);
                break;
            case 0x69:
                LoadRegister(ref _hl.Low, ref _bc.Low);
                break;
            case 0x6A:
                LoadRegister(ref _hl.Low, ref _de.High);
                break;
            case 0x6B:
                LoadRegister(ref _hl.Low, ref _de.Low);
                break;
            case 0x6C:
                LoadRegister(ref _hl.Low, ref _hl.High);
                break;
            case 0x6D:
                LoadRegister(ref _hl.Low, ref _hl.Low);
                break;
            case 0x70:
                WriteMemoryLocation(ref _bc.High);
                break;
            case 0x71:
                WriteMemoryLocation(ref _bc.Low);
                break;
            case 0x72:
                WriteMemoryLocation(ref _de.High);
                break;
            case 0x73:
                WriteMemoryLocation(ref _de.Low);
                break;
            case 0x74:
                WriteMemoryLocation(ref _hl.High);
                break;
            case 0x75:
                WriteMemoryLocation(ref _hl.Low);
                break;
            case 0x77:
                WriteMemoryLocation(ref _af.High);
                break;
            case 0x78:
                LoadRegister(ref _af.High, ref _bc.High);
                break;
            case 0x79:
                LoadRegister(ref _af.High, ref _bc.Low);
                break;
            case 0x7A:
                LoadRegister(ref _af.High, ref _de.High);   
                break;
            case 0x7B:
                LoadRegister(ref _af.High, ref _de.Low);
                break;
            case 0x7C:
                LoadRegister(ref _af.High, ref _hl.High);
                break;
            case 0x7D:
                LoadRegister(ref _af.High, ref _hl.Low);
                break;
            case 0xE0:
                WriteImmediateHighMemoryLocationA();
                break;
            case 0xE2:
                WriteHighMemoryLocationA();
                break;
            case 0xEA:
                WriteImmediateMemoryLocationA();
                break;
            case 0xF0:
                LoadImmediateHighMemoryLocationA();
                break;
            case 0xF2:
                LoadHighMemoryLocationA();
                break;
            case 0xFA:
                LoadImmediateMemoryLocationA();
                break;
        }
    }

    private void HlInc()
    {
        var tempHl = Hl;
        tempHl.Word++;
        Hl = tempHl;
    }
    
    private void HlDec()
    {
        var tempHl = Hl;
        tempHl.Word--;
        Hl = tempHl;
    }
    
    //Load Instructions
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
    
    //8-bit arithmetic instructions
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

    //Stack manipulation instructions
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
    
    //Miscellaneous instructions
    private void NoOperation() => Pc++;
}