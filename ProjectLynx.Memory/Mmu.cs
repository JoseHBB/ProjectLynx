namespace ProjectLynx.Memory;

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
    public void WriteByteToAddress(ushort address, byte data)
    {
        switch (address)
        {
            case <= 0x7FFF:
                _cartridge.WriteDataToRom(address, data); 
                break;
            case <= 0x9FFF:
                _videoRam[address - 0x8000] = data;
                break;    
            case <= 0xBFFF:
                _cartridge.WriteDataToExternalRam((ushort)(address - 0xA000), data);
                break;
            case <= 0xDFFF:
                _workRam[address - 0xC000] = data;
                break;
            case <= 0xFDFF:
                _workRam[address - 0xE000] = data;   //Same as work ram from to 0xE000 - 0xFDFF;
                break;                          
            case <= 0xFE9F:
                _objectAttributeMemory[address - 0xFE00] = data;
                break;
            case <= 0xFEFF:
                break;              //_notUsable[address - 0xFEA0], || ignora a escrita implementar depois o BUG OAM
            case <= 0xFF7F:
                _ioRegisters[address - 0xFF00] = data;
                break;
            case <= 0xFFFE:
                _highRam[address - 0xFF80] = data;
                break;
            default:
                _interruptEnableRegister = data;
                break;
        };
    }
}