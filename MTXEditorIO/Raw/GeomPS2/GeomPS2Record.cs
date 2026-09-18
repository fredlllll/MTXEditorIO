using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.IO;

namespace MTXEditorIO.Raw.GeomPS2
{
    public class GeomPS2Record
    {
        public const int RecordSize = 80;

        public uint prefix;      //0x00: 0x0000FF00
        public uint flags;       //0x04: often 0
        public uint magic;       //0x08: 0x4B189680
        public uint filler0;     //0x0C: 0xFFFFFFFF
        public Vec3 positionA;   //0x10
        public Vec3 positionB;   //0x1C
        public float radius;     //0x28
        public byte type;        //0x2C low byte: 0x01 (header record), 0x03, 0x05
        public uint dataOffset;  //0x30: offset into the data area (before the record table)
        public uint filler1;     //0x34: 0xFFFFFFFF
        public uint nextOffset;  //0x38: often the offset of the following record
        public uint unknown0;    //0x3C: often 0
        public uint checksum;    //0x40
        public uint unknown1;    //0x44: often 0
        public uint filler2;     //0x48: 0xFFFFFFFF
        public uint terminator;  //0x4C: 0x80808080

        public void ReadFrom(BinaryReader reader)
        {
            // 0x00
            prefix = reader.ReadUInt32();
            flags = reader.ReadUInt32();
            magic = reader.ReadUInt32();
            filler0 = reader.ReadUInt32();
            // 0x10
            positionA = reader.ReadStruct<Vec3>();
            positionB = reader.ReadStruct<Vec3>();
            radius = reader.ReadSingle();
            // 0x2C: bytes are [type, 0x00, 0x00, 0xFF]
            uint typeAndFiller = reader.ReadUInt32();
            type = (byte)(typeAndFiller & 0xFF);
            // 0x30
            dataOffset = reader.ReadUInt32();
            filler1 = reader.ReadUInt32();
            nextOffset = reader.ReadUInt32();
            unknown0 = reader.ReadUInt32();
            checksum = reader.ReadUInt32();
            unknown1 = reader.ReadUInt32();
            filler2 = reader.ReadUInt32();
            terminator = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return Output.ToString(this);
        }
    }
}