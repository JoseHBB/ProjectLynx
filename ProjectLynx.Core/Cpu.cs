namespace ProjectLynx.Core;

public class Cpu
{
    //Special Purpose Registers
    private ushort PC; //Program Counter
    private ushort SP; //Stack Pointer 
    private ushort IX; //Index Register X
    private ushort IY; //Index Register Y
    private byte I;    //Interrupt Page Address Register
    private byte R;    //Memory Refresher Register
    
    //Main Register Set
    //Accumulators
    private byte A; //Accumulator A
    private byte B; //Accumulator B
    private byte D; //Accumulator D
    private byte H; //Accumulator H
    //Flags
    private byte F; //Flag F
    private byte C; //Flag C
    private byte E; //Flag E
    private byte L; //Flag L
    
    //Alternate Register Set
    //Accumulators
    private byte A_; //Accumulator A'
    private byte B_; //Accumulator B'
    private byte D_; //Accumulator D'
    private byte H_; //Accumulator H'
    //Flags
    private byte F_; //Flag F'
    private byte C_; //Flag C'
    private byte E_; //Flag E'
    private byte L_; //Flag L'
}