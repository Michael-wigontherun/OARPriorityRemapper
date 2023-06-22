namespace OARPriorityGlobalLibrary
{
    public static class GL
    {
        public static string FileExtension = "_OARPriorirty.csv";

        public static string LogName = "";

        public static void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public static bool Close(string[] args, string logName)
        {
            LogName = logName;
            if (args.Length == 0)
            {
                GL.WriteLine("This program requires you to use console arguments.");
                GL.WriteLine("It only requires one single argument, the path to your data folder.");

                GL.WriteLine("Press Enter to close...");
                Console.ReadLine();
                return true;
            }
            else if (!File.Exists(Path.Combine(args[0], "Skyrim.esm")))
            {
                GL.WriteLine("Could not locate your data folder from your inputted path.");

                GL.WriteLine("Press Enter to close...");
                Console.ReadLine();
                return true;
            }
            return false;
        }
    }
}