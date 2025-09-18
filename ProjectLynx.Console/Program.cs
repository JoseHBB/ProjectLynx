using ProjectLynx.Core;
using ProjectLynx.Memory;
using ProjectLynx.Miscellaneous;

namespace ProjectLynx.Console;

internal static class Program 
{
    public static void Main(string[] args)
    {
        const string path = "D:/Desktop/gameboy rom/test_loads.gb";
        var rom = new Rom(path);
        rom.PrintHeaderData();

        var mmu = new Mmu(path);
        var cpu = new Cpu(mmu);

        cpu.Start();
    }
}