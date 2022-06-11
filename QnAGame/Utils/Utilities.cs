using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QnAGame.Utils
{
    internal static class Utilities
    {
        public static string CreateTitle(string title, bool uppercase)
        {
            var result = new StringBuilder();

            result.Append('=', title.Length + 8);
            result.Append($"\n||  {(uppercase? title.ToUpper() : title)}  ||\n");
            result.Append('=', title.Length + 8);

            return result.ToString();
        }

        public static void ShowTitle(string message, ConsoleColor color, bool uppercase)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(CreateTitle(message, uppercase));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ShowAlert(string message, ConsoleColor color)
        {
            Console.Clear();
            Console.ForegroundColor = color;
            Console.WriteLine(CreateTitle(message, true));
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            Console.ReadKey();
            Console.Clear();
            Console.CursorVisible = true;
        }

        public static int ValidateInputInt(string message, string errorMessage)
        {
            int value;
            while (true)
            {
                try
                {
                    Console.Write(message);
                    value = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.Clear();
                    ShowTitle(errorMessage, ConsoleColor.DarkRed, true);
                    Console.CursorVisible = false;
                    Console.ReadKey();
                    Console.Clear();
                    Console.CursorVisible = true;
                    continue;
                }
                Console.Clear();
                break;
            }

            return value;
        }

        public static string ValidateInput(string message, string errorMessage)
        {
            string value;
            while (true)
            {
                Console.Write(message);
                value = Console.ReadLine();

                if (string.IsNullOrEmpty(value))
                {
                    Console.Clear();
                    ShowTitle(errorMessage, ConsoleColor.DarkRed, true);
                    Console.CursorVisible = false;
                    Console.ReadKey();
                    Console.Clear();
                    Console.CursorVisible = true;
                    continue;
                }
                Console.Clear();
                break;
            }

            return value;
        }

        public static void SaveFile(string path, string fileName, string content)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllText($"{path}/{fileName}", content);
        }
    }
}
