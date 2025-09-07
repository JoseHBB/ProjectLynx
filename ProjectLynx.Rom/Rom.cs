namespace ProjectLynx.Rom;

public class Rom(string romPath)
{
    private readonly byte[] _data = File.ReadAllBytes(romPath);

    public void PrintHeaderData()
    {
        //Print Title of the game
        var index = 0x134;

        Console.WriteLine($"Name of the game:");
        while (index < 0x0143)
        {
            Console.Write((char)_data[index]);
            index++;
        }
        Console.WriteLine();
        
        //Print Nintendo Logo

        const char oneCharacter = '%';
        const char zeroCharacter = '.';
        
        string line1 = "", line2 = "", line3 = "", line4 = "";
        
        string top = "", bottom = "";
        
        index = 0x0104;
        var bitCount = 0;
        var topHalf = true;    
        
        while (index <= 0x0133)
        {
            var bitmapPart = _data[index];
            if ((bitmapPart & (1 << 7 - bitCount)) == 0)
            {
                if (topHalf)
                {
                    if (bitCount < 4)
                    {
                        line1 += zeroCharacter;
                    }
                    else
                    {
                        line2 += zeroCharacter;
                    }
                }
                else
                {
                    if (bitCount < 4)
                    {
                        line3 += zeroCharacter;
                    }
                    else
                    {
                        line4 += zeroCharacter;
                    }
                }
            }
            else
            {
                if (topHalf)
                {
                    if (bitCount < 4)
                    {
                        line1 += oneCharacter;
                    }
                    else
                    {
                        line2 += oneCharacter;
                    }
                }
                else
                {
                    if (bitCount < 4)
                    {
                        line3 += oneCharacter;
                    }
                    else
                    {
                        line4 += oneCharacter;
                    }
                }
            }

            if (bitCount == 7)
            {
                bitCount = 0;
            }
            else
            {
                bitCount++;
            }

            if (bitCount == 0)
            {
                index++;
                topHalf = !topHalf;
            }

            if ((index == 0x011B + 1) && bitCount == 0)
            {
                top = line1 + '\n' + line2 + '\n' + line3 + '\n' + line4;
                line1 = line2 = line3 = line4 = "";
            }
            
            if (index == 0x0134)
            {
                bottom = line1 + '\n' + line2 + '\n' + line3 + '\n' + line4;
            }
        }
        Console.WriteLine(top);
        Console.WriteLine(bottom);
    }
}
