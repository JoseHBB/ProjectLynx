using System.Diagnostics.CodeAnalysis;

namespace ProjectLynx.Mmu;

public class Mmu(string romPath)
{
    private Cartridge _cartridge = new Cartridge(romPath);
    private byte[] _videoRam = new byte[0x1FFF];
    private byte[] _workRam = new byte[0x1FFF];
    private byte[] _objectAttributeMemory = new byte[0x009F];
    //private byte[] _notUsable = new  byte[0x009F];
    private byte[] _ioRegisters = new byte[0x007F];
    private byte[] _highRam = new byte[0x007E];
    private byte _interruptEnableRegister;
    
    public byte GetByteFromAddress(ushort address)
    {
        return address switch
        {
            <= 0x7FFF => _cartridge.GetDataFromAddress(address),
            <= 0x9FFF => _videoRam[address - 0x8000],
            <= 0xBFFF => _cartridge.GetDataFromExternalRam((ushort)(address - 0xA000)),
            <= 0xDFFF => _workRam[address - 0xD000],
            <= 0xFDFF => _workRam[address - 0xE000], //Same as work ram from to 0xE000 - 0xFDFF;
            <= 0xFE9F => _objectAttributeMemory[address - 0xFE00],
            <= 0xFEFF => 0xFF,                       //_notUsable[address - 0xFEA0], || retorna 0xFF implementar depois o BUG OAM
            <= 0xFF7F => _ioRegisters[address - 0xFF00],
            <= 0xFFFE => _highRam[address - 0xFF80],
            _ => _interruptEnableRegister
        };
    }
}