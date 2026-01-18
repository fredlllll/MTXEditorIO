using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Util.Hashing
{
    public static class Crc32IEEE
    {
        private static readonly uint[] crcTable = new uint[256];

        static Crc32IEEE()
        {
            uint num = 3988292384u;
            for (int i = 0; i < 256; i++)
            {
                uint num2 = (uint)i;
                for (int num3 = 8; num3 > 0; num3--)
                {
                    num2 = (((num2 & 1) != 1) ? (num2 >> 1) : ((num2 >> 1) ^ num));
                }
                crcTable[i] = num2;
            }
        }

        public static uint Hash(byte[] bytes)
        {
            return Hash(bytes, bytes.Length);
        }
        public static uint Hash(byte[] bytes, int length)
        {
            uint num = uint.MaxValue;
            for (int i = 0; i < length; i++)
            {
                num = ((num >> 8) & 0xFFFFFF) ^ crcTable[(num ^ bytes[i]) & 0xFF];
            }
            return num;
        }
    }
}
