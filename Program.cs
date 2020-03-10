using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XorEncoding;

namespace XorEncoding
{
    class Program
    {
        enum Colors
        {
            Standard = ConsoleColor.White,
            Output = ConsoleColor.Green,
            Warning = ConsoleColor.Magenta,
            Input = ConsoleColor.Red,
        };

        [STAThread]
        static void Main()
        {
            Console.ForegroundColor = (ConsoleColor)Colors.Standard;
            while (true)
            {
                Console.WriteLine("\n");
                String msg = GetConsoleInput("Input text to be encoded: ");
                if (String.IsNullOrWhiteSpace(msg))
                {
                    WarningToConsole("Please input a message with some content next time");
                    continue;
                }
                if (!msg.CheckStr())
                {
                    WarningToConsole("Please use only legal characters:", true);
                    OutputToConsole(EncoderDecoder.CharsString);
                    continue;
                }
                String key = GetConsoleInput("Encryption Key: ");
                if (String.IsNullOrWhiteSpace(key))
                {
                    WarningToConsole("Please input a key with some content next time");
                    continue;
                }
                if (!key.CheckStr())
                {
                    WarningToConsole("Please use only legal characters:", true);
                    OutputToConsole(EncoderDecoder.CharsString);
                    continue;
                }
                if (key.Length > 119)
                {
                    WarningToConsole("Please make the key less than 120 chars long");
                    continue;
                }


                //concat the key to increase its length if it is too short. Our presumed inputkey is shorter than 119
                key += "M0ZMEy5ZcVNjcR8PMBiM9zVqsDqCNXD6k0YJ95uK5m1GVOWOq7MibY0See4hoB4jw8i9eZqwwISET24hdjsrC5moo2RhbpFaVM08Kd2JLMtxLDDZznZxawqB7g7votGL".Substring(0,119-key.Length);
                if (!key.CheckStr()) {
                    throw new FormatException("Please warn the developer that his key appending string contains illegal characters");
                }
                //ensure that the key is a prime number of inequal length to the message
                //without this step there is no guarantee that a different number of repeats won't result in different outcomes
                if (key.Length == msg.Length || key.Length > 119) key = key.Substring(0, 117);

                //prepare primes for the next step
                List<Int32> primes = new List<Int32> { 2 };
                for (Int32 i = 3; primes.Count() < 1000; i += 2)
                {
                    foreach (Int32 prime in primes)
                    {
                        if (i % prime == 0) break;
                        if (prime * prime > i)
                        {
                            primes.Add(i);
                            break;
                        }
                    }
                }

                //try to ensure that the number of repeats changes practically every time with any slight change of key value, no matter how slight
                Int32 repeats = 1;
                for (Int32 i = 0; i < key.Length; i++)
                {
                    for (Int32 j = 0; j < key[i]; j++)
                    {
                        repeats *= primes[i];
                        repeats %= 0b1_00000000_00000000;
                        if (repeats < 1) repeats = 1;
                    }
                }
                repeats += 0b11100000_00000000;
                String encoded = msg.EnOrDecode(key, repeats);

                OutputToConsole("Encryption Repeats: ", repeats.ToString("N0"));
                Console.Write("Encoded string: \"");
                OutputToConsole(encoded, true);
                Console.Write("\".\ntype 'y' to copy to clipboard:");
                if (Console.ReadKey().KeyChar == 'y') System.Windows.Clipboard.SetText(encoded);
            }
        }

        /// <summary>
        /// Gets String input from the user
        /// </summary>
        /// <param name="msg">The message to prompt the user with</param>
        /// <returns></returns>
        static String GetConsoleInput(String msg = "")
        {
            Console.ForegroundColor = (ConsoleColor)Colors.Standard;
            Console.Write(msg);
            Console.ForegroundColor = (ConsoleColor)Colors.Input;
            String input = Console.ReadLine();
            Console.ForegroundColor = (ConsoleColor)Colors.Standard;

            return input;
        }
        /// <summary>
        /// Show a colored message to the user
        /// </summary>
        /// <param name="info">The message to show to the user</param>
        /// <param name="noLine">Whether or not following console writes should appear on a new line</param>
        /// <returns></returns>
        static void OutputToConsole(String info, Boolean noLine = false)
        {
            Console.ForegroundColor = (ConsoleColor)Colors.Output;
            if (noLine) Console.Write(info);
            else Console.WriteLine(info);
            Console.ForegroundColor = (ConsoleColor)Colors.Standard;
        }
        /// <summary>
        /// Show a colored message to the user, giving standard coloring to the msg parameter.
        /// </summary>
        /// <param name="msg">The message to show to the user in standard coloring</param>
        /// <param name="info">The information to be provided</param>
        /// <param name="noLine">Whether or not a later message should appear on a new line</param>
        static void OutputToConsole(String msg, String info, Boolean noLine = false)
        {
            Console.Write(msg);
            OutputToConsole(info, noLine);
        }
        static void WarningToConsole(String msg, Boolean noLine = false)
        {
            Console.ForegroundColor = (ConsoleColor)Colors.Warning;
            if (noLine) Console.Write(msg);
            else Console.WriteLine(msg);
            Console.ForegroundColor = (ConsoleColor)Colors.Standard;
        }
    }
}
