namespace ProjectLynx.Console;

internal static class Program 
{
    public static void Main(string[] args)
    {
        var rom = new Rom.Rom("D:/Rider Projects/Tumble Pop (USA, Europe).gb");
        
        rom.PrintHeaderData();
    }
}