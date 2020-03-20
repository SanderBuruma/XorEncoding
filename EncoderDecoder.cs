using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XorEncoding
{

	static class EncoderDecoder
	{
		/// <summary>
		/// The Base64 base chars
		/// </summary>
		public static readonly String CharsString;

		/// <summary>
		/// special char translation to and from base64compliant baseword
		/// </summary>
		public static readonly Dictionary<String, String> WordSpecialCodes;

		static EncoderDecoder()
		{
			WordSpecialCodes = new Dictionary<string, string>
			{
				{"&", "ZZZ.ZAD"},
				{"@", "ZZZ.ZAT"},
				{"[", "ZZZ.ZBC"},
				{"]", "ZZZ.ZBC"},
				{"}", "ZZZ.ZCC"},
				{",", "ZZZ.ZCM"},
				{"{", "ZZZ.ZCO"},
				{"$", "ZZZ.ZDLR"},
				{"!", "ZZZ.ZEM"},
				{">", "ZZZ.ZGT"},
				{"#", "ZZZ.ZHT"},
				{"<", "ZZZ.ZLT"},
				{"=", "ZZZ.ZIS"},
				{"-", "ZZZ.ZMI"},
				{")", "ZZZ.ZPC"},
				{"+", "ZZZ.ZPL"},
				{"(", "ZZZ.ZPO"},
				{"%", "ZZZ.ZPRC"},
				{"^", "ZZZ.ZPW"},
				{"?", "ZZZ.ZQM"},
				{"_", "ZZZ.ZUS"},
				{"*", "ZZZ.ZXX"},
				{";", "ZZZ.ZX1"},
				{":", "ZZZ.ZX2"},
				{"'", "ZZZ.ZX3"},
				{"\"", "ZZZ.ZX4"},
				{"|", "ZZZ.ZX5"},
				{"\\", "ZZZ.ZX6"},
				{"/", "ZZZ.ZX7"},
				{"~", "ZZZ.ZX8"},
				{"`", "ZZZ.ZX9"},
			};

			StringBuilder strBlr = new StringBuilder();
			strBlr.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 .");
			foreach (KeyValuePair<string, string> wsc in WordSpecialCodes)
			{
				strBlr.Append(wsc.Key);
			}

			CharsString = strBlr.ToString();
		}

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
			//replace special characters to make them compliant with Base64
			foreach (var wsc in WordSpecialCodes)
			{
				if (msg.Contains(wsc.Key))
				{
					msg = msg.Replace(wsc.Key, " " + wsc.Value + " ");
				}
			}

			//conver to int array
			if (repeats % 2 == 0)
				repeats++;
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

			//reconvert special characters into their real form.
			var returnStr = strBldr.ToString();
			foreach (var wsc in WordSpecialCodes)
			{
				if (returnStr.Contains(" " + wsc.Value + " "))
				{
					returnStr = returnStr.Replace(" " + wsc.Value + " ", wsc.Key);
				}
			}

			return returnStr;
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
