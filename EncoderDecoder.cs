using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XorEncoding
{

   static class EncoderDecoder
   {
      public static readonly String CharsString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 .";
      /// <summary>
      /// Converts chars to a custom char array
      /// </summary>
      /// <param name="that">The string to be encoded. May only contain characters found directly on the MacBook pro.</param>
      /// <returns></returns>
      private static Int32[] ToEncodedIntArr(this String that)
      {
         List<Int32> rtn = new List<Int32> { };
         Char[] thatChars = that.ToCharArray();
         foreach (var c in thatChars)
         {
            rtn.Add(CharsString.IndexOf(c));
         }

         return rtn.ToArray();
      }

      /// <summary>
      /// This encodes when first run and decodes when run again on its own result with the same key and repeats
      /// </summary>
      /// <param name="msg">The message to be en/decoded</param>
      /// <param name="key">The encyrption key</param>
      /// <param name="repeats">The number of repitions to go through to encode it</param>
      /// <returns></returns>
      public static String EnOrDecode(this String msg, String key, Int32 repeats = 3)
      {
         if (repeats % 2 == 0) repeats++;
         Int32[] msgInts = msg.ToEncodedIntArr();
         Int32[] keyInts = key.ToEncodedIntArr();
         Int32 maxLen = Math.Max(msg.Length, key.Length);
         for (Int32 i = 0; i < maxLen * repeats; i++)
         {
            msgInts[i % msgInts.Length] = msgInts[i % msgInts.Length] ^ keyInts[i % keyInts.Length];
         }

         //convert back to string
         StringBuilder strBldr = new StringBuilder(msg.Length);
         for (Int32 i = 0; i < msgInts.Length; i++)
         {
            strBldr.Append(CharsString[msgInts[i]]);
         }
         return strBldr.ToString();
      }
      /// <summary>
      /// Returns true if the string contains only the characters in its own hardcoded Chars string
      /// </summary>
      /// <param name="that">The string to check</param>
      /// <returns></returns>
      public static Boolean CheckStr(this string that)
      {
         foreach (var @char in that.ToCharArray())
         {
            if (CharsString.IndexOf(@char) == -1) return false;
         }
         return true;
      }
   }
}
