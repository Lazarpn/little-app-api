using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Common.Helpers;

public static class Utilities
{
    public static string GenerateAlphaNumericCode(int codeLength)
    {
        return GenerateCode(codeLength, "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789");
    }

    public static string GenerateNumericCode(int codeLength)
    {
        return GenerateCode(codeLength, "1234567890");
    }

    private static string GenerateCode(int codeLength, string valid)
    {

        StringBuilder result = new();
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (codeLength-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                result.Append(valid[(int)(num % (uint)valid.Length)]);
            }
        }

        return result.ToString();
    }

}
