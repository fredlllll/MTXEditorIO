using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.Shared
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct OneByteBoolean
    {
        public byte value;

        public static implicit operator bool(OneByteBoolean obb)
        {
            return obb.value != 0;
        }

        public static implicit operator OneByteBoolean(bool val)
        {
            return new OneByteBoolean() { value = val ? byte.MaxValue : byte.MinValue };
        }

        public override string ToString()
        {
            return $"[{(bool)this}]{value}";
        }
    }
}
