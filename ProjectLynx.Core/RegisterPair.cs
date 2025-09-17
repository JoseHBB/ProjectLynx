using System.Runtime.InteropServices;

namespace ProjectLynx.Core;

[StructLayout(LayoutKind.Explicit)]
public struct RegisterPair
{
    [FieldOffset(0)]
    public ushort Word;
    
    [FieldOffset(0)]
    public byte Low;

    [FieldOffset(1)] 
    public byte High;
}