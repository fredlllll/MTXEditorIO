using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Util
{
    public static class StringUtils
    {
        /// <summary>
        /// get a string from bytes.
        /// be aware this searches for first non 0 char from the end, so you could end up with 0 chars in the string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetString(byte[] bytes)
        {
            int strlen = 0;
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                if (bytes[i] != 0)
                {
                    strlen = i + 1;
                    break;
                }
            }
            string retval = Encoding.ASCII.GetString(bytes, 0, strlen);
            return retval;
        }

        /// <summary>
        /// does the same as GetString but searches for \0 from the start, so it might truncate data, but you will not have \0 in your strings
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetStringSafe(byte[] bytes)
        {
            int strlen = bytes.Length;
            for (int i = 0; i < bytes.Length; ++i)
            {
                if (bytes[i] == 0)
                {
                    strlen = i;
                    break;
                }
            }
            string retval = Encoding.ASCII.GetString(bytes, 0, strlen);
            return retval;
        }

        /// <summary>
        /// get fixed length bytes of a string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="bytesLength"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string str, int bytesLength)
        {
            byte[] retval = new byte[bytesLength];
            if (str != null)
            {
                try
                {
                    Encoding.ASCII.GetBytes(str, 0, str.Length, retval, 0);
                }
                catch (ArgumentException) //not enough space in retval for the string
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(str);
                    Array.Copy(bytes, retval, bytesLength);
                }
            }
            return retval;
        }

        /// <summary>
        /// return bytes of a string, no 0 terminator
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
    }
}
