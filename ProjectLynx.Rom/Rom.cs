namespace ProjectLynx.Rom;

public class Rom(string romPath)
{
    private readonly byte[] _data = File.ReadAllBytes(romPath);

    public byte GetByteFromAddress(ushort address)
    {
        return _data[address];
    }

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

        if (_data[0x0146] != 0x03)
        {
            Console.WriteLine("This game does not support Super GameBoy functions");
        }
        else
        {
            Console.WriteLine("This game does support Super GameBoy functions");
        }
        
        if (_data[0x0147] == 0x00)
        {
            Console.WriteLine("This game is ROM only");
        }
        else
        {
            Console.WriteLine("This game is not ROM only");
        }
        
        var romSize = _data[0x0148];
        
        Console.WriteLine($"The ROM size is: {32 * (1 << romSize)} KiB");

        switch (_data[0x0149])
        {
            case 0x00:
                Console.WriteLine("This has no RAM");
                break;
            case 0x01:
                Console.WriteLine("Unused");
                break;
            case 0x02:
                Console.WriteLine("This has 8Kib of RAM.");
                break;
            default:
                Console.WriteLine("The RAM byte is wrong.");
                break;
        }

        if (_data[0x014A] == 0x00)
        {
            Console.WriteLine("This game is for Japan and overseas.");
        }
        else
        {
            Console.WriteLine("This game is for overseas only.");
        }
        
        if (_data[0x014B] == 0x01)
        {
            Console.WriteLine("This game is published by Nintendo.");
        }
        else
        {
            Console.WriteLine("This game is NOT published by Nintendo.");
        }
        
        Console.WriteLine($"The Mask ROM version is: {_data[0x014C]}");

        byte checksum = 0;

        for (var address = 0x0134; address <= 0x014C; address++)
        {
            checksum = (byte)(checksum - _data[address] - 1);
        }

        Console.WriteLine($"The checksum is: {checksum}");
        
        if (checksum == _data[0x014D])
        {
            Console.WriteLine("The checksum is correct");
        }
        else
        {
            Console.WriteLine("The checksum is not correct");
        }

        index = 0;

        ushort globalChecksum = 0;
        
        while (index < _data.Length)
        {
            if (index == 0x014E)
            {
                index += 2;
                continue;
            }

            globalChecksum += _data[index];
            
            index++;
        }

        var globalCheck = (ushort)(_data[0x014E] << 8) | _data[0x014F];

        if (globalChecksum == globalCheck)
        {
            Console.WriteLine("The global checksum is correct");
        }
        else
        {
            Console.WriteLine("The global checksum is not correct");
        }
    }
}
