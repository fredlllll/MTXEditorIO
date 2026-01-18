// PreTool, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// fuQ.QSharp.Lzss
using System.IO;
using System.Text;

namespace MTXEditorIO.Util.Compression
{
    /// <summary>
    /// taken from the PreTool because i cant be bothered to understand what the hell this does right now. maybe at a later date i will reimplement it
    /// </summary>
    public class Lzss
    {
        /*private const int N = 4096;

		private const int F = 18;

		private const int THRESHOLD = 2;

		private const int NIL = 4096;*/

        private byte[] _text_buf;

        private int _match_position;

        private int _match_length;

        private int[] _lson;

        private int[] _rson;

        private int[] _dad;

        public Lzss()
        {
            _text_buf = new byte[4113];
            _lson = new int[4097];
            _rson = new int[4353];
            _dad = new int[4097];
        }

        public byte[] Decompress(byte[] compressed)
        {
            using MemoryStream input = new MemoryStream(compressed);
            using MemoryStream memoryStream = new MemoryStream();
            int num = Decompress(input, memoryStream);
            memoryStream.Seek(0L, SeekOrigin.Begin);
            byte[] array = new byte[num];
            memoryStream.Read(array, 0, num);
            new StringBuilder(Encoding.UTF8.GetString(array));
            return array;
        }

        public int Decompress(Stream input, Stream output)
        {
            byte[] array = new byte[4113];
            for (int i = 0; i < 4078; i++)
            {
                array[i] = 32;
            }
            int num = 4078;
            uint num2 = 0u;
            int num3 = 0;
            while (input.Position < input.Length)
            {
                if (((num2 >>= 1) & 0x100) == 0)
                {
                    num2 = (uint)(input.ReadByte() & 0xFF | 0xFF00);
                }
                if ((num2 & 1) == 1)
                {
                    int num4 = input.ReadByte() & 0xFF;
                    output.WriteByte((byte)num4);
                    num3++;
                    array[num++] = (byte)num4;
                    num &= 0xFFF;
                    continue;
                }
                int i = input.ReadByte() & 0xFF;
                int num5 = input.ReadByte() & 0xFF;
                i |= (num5 & 0xF0) << 4;
                num5 = (num5 & 0xF) + 2;
                for (int j = 0; j <= num5; j++)
                {
                    int num4 = array[i + j & 0xFFF];
                    output.WriteByte((byte)num4);
                    num3++;
                    array[num++] = (byte)num4;
                    num &= 0xFFF;
                }
            }
            return num3;
        }

        public byte[] Compress(byte[] data)
        {
            using MemoryStream input = new MemoryStream(data);
            using MemoryStream memoryStream = new MemoryStream();
            int num = Compress(input, memoryStream);
            memoryStream.Seek(0L, SeekOrigin.Begin);
            byte[] array = new byte[num];
            memoryStream.Read(array, 0, num);
            return array;
        }

        public int Compress(Stream input, Stream output)
        {
            int num = 0;
            byte[] array = new byte[17];
            InitTree();
            array[0] = 0;
            byte b;
            int num2 = b = 1;
            int num3 = 0;
            int num4 = 4078;
            for (int i = num3; i < num4; i++)
            {
                _text_buf[i] = 32;
            }
            int j;
            for (j = 0; j < 18; j++)
            {
                if (input.Position == input.Length)
                {
                    break;
                }
                byte b2 = (byte)(input.ReadByte() & 0xFF);
                _text_buf[num4 + j] = b2;
            }
            if (j == 0)
            {
                return 0;
            }
            for (int i = 1; i <= 18; i++)
            {
                InsertNode(num4 - i);
            }
            InsertNode(num4);
            do
            {
                if (_match_length > j)
                {
                    _match_length = j;
                }
                if (_match_length <= 2)
                {
                    _match_length = 1;
                    array[0] |= b;
                    array[num2++] = _text_buf[num4];
                }
                else
                {
                    array[num2++] = (byte)_match_position;
                    array[num2++] = (byte)(_match_position >> 4 & 0xF0 | _match_length - 3);
                }
                int i;
                if ((b <<= 1) == 0)
                {
                    for (i = 0; i < num2; i++)
                    {
                        output.WriteByte(array[i]);
                        num++;
                    }
                    array[0] = 0;
                    num2 = b = 1;
                }
                int match_length = _match_length;
                for (i = 0; i < match_length; i++)
                {
                    if (input.Position == input.Length)
                    {
                        break;
                    }
                    byte b2 = (byte)(input.ReadByte() & 0xFF);
                    DeleteNode(num3);
                    _text_buf[num3] = b2;
                    if (num3 < 17)
                    {
                        _text_buf[num3 + 4096] = b2;
                    }
                    num3 = num3 + 1 & 0xFFF;
                    num4 = num4 + 1 & 0xFFF;
                    InsertNode(num4);
                }
                while (i++ < match_length)
                {
                    DeleteNode(num3);
                    num3 = num3 + 1 & 0xFFF;
                    num4 = num4 + 1 & 0xFFF;
                    if (--j != 0)
                    {
                        InsertNode(num4);
                    }
                }
            }
            while (j > 0);
            if (num2 > 1)
            {
                for (int i = 0; i < num2; i++)
                {
                    output.WriteByte(array[i]);
                    num++;
                }
            }
            return num;
        }

        private void InitTree()
        {
            for (int i = 4097; i <= 4352; i++)
            {
                _rson[i] = 4096;
            }
            for (int i = 0; i < 4096; i++)
            {
                _dad[i] = 4096;
            }
        }

        private void InsertNode(int r)
        {
            int num = 1;
            int num2 = 4097 + _text_buf[r];
            _rson[r] = _lson[r] = 4096;
            _match_length = 0;
            while (true)
            {
                if (num >= 0)
                {
                    if (_rson[num2] == 4096)
                    {
                        _rson[num2] = r;
                        _dad[r] = num2;
                        return;
                    }
                    num2 = _rson[num2];
                }
                else
                {
                    if (_lson[num2] == 4096)
                    {
                        _lson[num2] = r;
                        _dad[r] = num2;
                        return;
                    }
                    num2 = _lson[num2];
                }
                int i;
                for (i = 1; i < 18; i++)
                {
                    if ((num = _text_buf[r + i] - _text_buf[num2 + i]) != 0)
                    {
                        break;
                    }
                }
                if (i > _match_length)
                {
                    _match_position = num2;
                    if ((_match_length = i) >= 18)
                    {
                        break;
                    }
                }
            }
            _dad[r] = _dad[num2];
            _lson[r] = _lson[num2];
            _rson[r] = _rson[num2];
            _dad[_lson[num2]] = r;
            _dad[_rson[num2]] = r;
            if (_rson[_dad[num2]] == num2)
            {
                _rson[_dad[num2]] = r;
            }
            else
            {
                _lson[_dad[num2]] = r;
            }
            _dad[num2] = 4096;
        }

        private void DeleteNode(int p)
        {
            if (_dad[p] == 4096)
            {
                return;
            }
            int num;
            if (_rson[p] == 4096)
            {
                num = _lson[p];
            }
            else if (_lson[p] == 4096)
            {
                num = _rson[p];
            }
            else
            {
                num = _lson[p];
                if (_rson[num] != 4096)
                {
                    do
                    {
                        num = _rson[num];
                    }
                    while (_rson[num] != 4096);
                    _rson[_dad[num]] = _lson[num];
                    _dad[_lson[num]] = _dad[num];
                    _lson[num] = _lson[p];
                    _dad[_lson[p]] = num;
                }
                _rson[num] = _rson[p];
                _dad[_rson[p]] = num;
            }
            _dad[num] = _dad[p];
            if (_rson[_dad[p]] == p)
            {
                _rson[_dad[p]] = num;
            }
            else
            {
                _lson[_dad[p]] = num;
            }
            _dad[p] = 4096;
        }
    }
}