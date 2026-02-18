using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public struct IndentationModifier
    {
        public bool currentLine;
        public int amount;

        public static IndentationModifier None
        {
            get
            {
                return new IndentationModifier()
                {
                    currentLine = false,
                    amount = 0
                };
            }
        }

        public static IndentationModifier ThisIndent
        {
            get
            {
                return new IndentationModifier()
                {
                    currentLine = true,
                    amount = 1
                };
            }
        }

        public static IndentationModifier ThisUnindent
        {
            get
            {
                return new IndentationModifier()
                {
                    currentLine = true,
                    amount = -1
                };
            }
        }

        public static IndentationModifier NextIndent
        {
            get
            {
                return new IndentationModifier()
                {
                    currentLine = false,
                    amount = 1
                };
            }
        }

        public static IndentationModifier NextUnindent
        {
            get
            {
                return new IndentationModifier()
                {
                    currentLine = false,
                    amount = -1
                };
            }
        }
    }
}
