using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.SkaPC
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SkaPCShortPair
    {
        public ushort unknown1;
        public ushort unknown2;
    }

    public class SkaPC : IReadableWriteableFromStream
    {
        public SkaPCHeader header;
        public SkaPCShortPair[] shortPairs;

        public void ReadFrom(Stream stream)
        {
            var reader = new BinaryReader(stream, Encoding.ASCII, true);

            header = reader.ReadStruct<SkaPCHeader>();
            Console.WriteLine(Output.ToString(header));
            shortPairs = new SkaPCShortPair[header.numShortPairs];
            for (int i = 0; i < header.numShortPairs; i++)
            {
                shortPairs[i] = reader.ReadStruct<SkaPCShortPair>();
                Console.WriteLine(Output.ToString(shortPairs[i]));
            }

            //what follows seems to be a header and then keyframe data. each keyframe has a small header itself and then data
            //right now i cant quickly make sense of this without spending a lot of time on it, so i will leave this as a TODO
        }

        public void WriteTo(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
