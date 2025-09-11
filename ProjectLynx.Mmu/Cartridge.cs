namespace ProjectLynx.Mmu;

public class Cartridge(string romPath)
{
    private readonly byte[] _data = File.ReadAllBytes(romPath);
    private byte[] _externalRam =  new byte[0x1FFF];
    public byte GetDataFromAddress(ushort address)
    {
        return  _data[address];
    }

    public void WriteDataToRom(ushort address, byte data)
    {
        return;
    }
    public byte GetDataFromExternalRam(ushort address)
    {
        return _externalRam[address];
    }
    public void WriteDataToExternalRam(ushort address, byte data)
    {
        _externalRam[address] = data;
    }
}