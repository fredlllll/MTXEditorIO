using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.TexPC
{
    public class TexPCImg
    {
        public TexPCImgHeader header;
        public TexPCImgLevel? level;

        public void ReadFrom(StructReader reader)
        {
            header = reader.ReadStruct<TexPCImgHeader>();

            level = new TexPCImgLevel();
            level.ReadFrom(reader);

            if (header.dxt != 0x01)
            {
                //conversion of other formats not supported, but doesnt have to be here in the load logic
                //throw new Exception("DXT{header.dxt} is not supported");
            }
        }
    }
}
