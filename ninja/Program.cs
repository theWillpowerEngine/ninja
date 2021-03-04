using System;
using System.IO;
using System.Linq;
using ninja.lang;

namespace ninja
{
    class Program
    {
        private static string currentDir = Directory.GetCurrentDirectory();

        static void Evaluate(string code)
        {
            try
            {
                var obj = Ninja.Parse(code);
                obj.Evaluate();
            } 
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("ERROR:  " + ex.Message);
                Console.WriteLine();
            }
        }

        static void CommandMode(string[] args)
        {
            if (args.Length == 0)
                return;

            switch(args[0].ToLower())
            {
                case "exit":
                    return;
            }
        }

        static void SmartMode()
        {
            var ninjaFiles = Directory.EnumerateFiles(currentDir, "*.ninja").ToList();
            string code;

            if (ninjaFiles.Count == 1)
            {
                code = File.ReadAllText(ninjaFiles[0]);
                Evaluate(code);
            }
            else if (ninjaFiles.Count > 1)
            {
            retryMultiFile: 
                Console.WriteLine("\nI found more than one Ninja file, which one do you want to evaluate?");
                int i = 1;
                ninjaFiles.ForEach(f => {
                    if (i < 10) Console.WriteLine($"\t{i++}) {Path.GetFileName(f)}");
                });

                var key = Console.ReadKey();
                int idx = int.Parse(key.KeyChar.ToString());
                if(idx > ninjaFiles.Count || idx == 0)
                {
                    Console.WriteLine("That's not a valid option, try again (y/N)?");
                    key = Console.ReadKey();
                    if (key.KeyChar.ToString().ToLower() == "y")
                    {
                        Console.Clear();
                        goto retryMultiFile;
                    }
                }
                Console.WriteLine();
                code = File.ReadAllText(ninjaFiles[idx - 1]);
                Evaluate(code);
            }
            else
            {
                Console.WriteLine("ninja has entered interactive console mode...");
                Console.WriteLine("  You can now use ninja ... console commands (omit the 'ninja' part)");
                Console.WriteLine("  type 'exit' (no quotes) to exit, or press Ctrl-C\n");

                string cmd = "";

                while(cmd != "exit")
                {
                    Console.Write(":)  ");
                    cmd = Console.ReadLine().ToLower();
                    Console.WriteLine();
                    CommandMode(cmd.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                CommandMode(args);
            }
            else if (args.Length == 1)
            {
                if (Directory.Exists(args[0]))
                {
                    currentDir = args[0];
                    currentDir = currentDir.TrimEnd('/', '\\') + "/";
                    SmartMode();
                }
  
                else if (Directory.Exists(currentDir + args[0]))
                { 
                    currentDir = currentDir + args[0];
                    currentDir = currentDir.TrimEnd('/', '\\') + "/";
                    SmartMode();
                } 
                else
                {
                    CommandMode(args);
                }
            }
            else
            {
                SmartMode();
            }
        }
    }
}
