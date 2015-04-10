using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MergeTool.Console
{
    public static class Helper
    {
        static readonly object Locker = new object();

        public static IList<Contact> ReadFromFile(string filePath)
        {
            var result = new List<Contact>();

            if (!File.Exists(filePath))
            {
                LogWarning("File '{0}' not found", filePath);
            }
            else
            {
                try
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        string json = reader.ReadToEnd();
                        result = JsonConvert.DeserializeObject<List<Contact>>(json);
                    }
                }
                catch (Exception exc)
                {
                    LogError("Error rading json file '{0}'. Error: {1}", filePath, exc);
                }
            }

            return result;
        }

        public static void LogError(string format, params object[] arg)
        {
            lock (Locker)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(format, arg);
                System.Console.ResetColor();
            }
        }

        public static void LogWarning(string format, params object[] arg)
        {
            lock (Locker)
            {
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine(format, arg);
                System.Console.ResetColor();
            }
        }

        public static void LogInfo(string format, params object[] arg)
        {
            lock (Locker)
            {
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine(format, arg);
                System.Console.ResetColor();
            }

        }
    }
}
