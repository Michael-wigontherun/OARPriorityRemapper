using OARPriorityGlobalLibrary;
using System.Diagnostics;
using System.Text.Json;

namespace OARPriorityReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            if (GL.Close(args, "OARPriorityReader.txt")) return;

            Directory.CreateDirectory("OARBaseData");
            Directory.CreateDirectory("OAROverrideData");
            
            string dataPath = args[0];

            GL.WriteLine("Looking for OpenAnimationReplacer folders.");

            IEnumerable<string> OpenAnimationReplacerFolders = Directory.GetDirectories(Path.Combine(dataPath, "Meshes"),
                "OpenAnimationReplacer",
                SearchOption.AllDirectories);
            foreach (string OpenAnimationReplacerFolder in OpenAnimationReplacerFolders)
            {
                GL.WriteLine($"Looking for Mod Folders inside {OpenAnimationReplacerFolder.Replace(dataPath, "")}.");

                IEnumerable<string> modFolders = Directory.GetDirectories(OpenAnimationReplacerFolder,
                    "*",
                    SearchOption.TopDirectoryOnly);
                foreach (var modFolder in modFolders)
                {
                    string modFolderWithoutDataPath = modFolder.Replace(dataPath, "");
                    if (modFolderWithoutDataPath[0].Equals('\\')) modFolderWithoutDataPath = modFolderWithoutDataPath.Remove(0, 1);

                    string modName = Path.GetFileName(modFolder);
                    GL.WriteLine("\n\n");
                    GL.WriteLine($"Reading {modName}.");

                    IEnumerable<string> modSubFolders = Directory.GetDirectories(modFolder,
                        "*",
                        SearchOption.TopDirectoryOnly);

                    if (!modSubFolders.Any()) continue;

                    List<SubFolderData> subData = new List<SubFolderData>();

                    foreach (string modSubFolder in modSubFolders)
                    {
                        string configPath = Path.Combine(modSubFolder, "config.json");
                        if (!File.Exists(configPath))
                        {
                            GL.WriteLine($"{Path.GetFileName(modSubFolder)} is missing a config.json file.");
                            continue;
                        }
                        
                        using (JsonDocument document = JsonDocument.Parse(File.ReadAllText(configPath)))
                        {
                            JsonElement root = document.RootElement;
                            if (root.TryGetProperty("priority", out JsonElement priority))
                            {
                                if (priority.ValueKind.Equals(JsonValueKind.Number))
                                {
                                    subData.Add(new SubFolderData(priority.GetInt32(), Path.GetFileName(modSubFolder)));
                                }
                                else
                                {
                                    Console.WriteLine($"{Path.GetFileName(modSubFolder)}'s prioity is not an int.");
                                }
                            }
                        }
                    }

                    subData = subData.OrderBy(x => x.Priority).ToList();

                    using (var writer = new StreamWriter($"OARBaseData\\{modName}{GL.FileExtension}", false))
                    {
                        writer.WriteLine(modFolderWithoutDataPath);
                        foreach (var data in subData)
                        {
                            GL.WriteLine($"{data.Name} has priority {data.Priority}.");
                            writer.WriteLine($"{data.Name};{data.Priority};");
                        }
                    }
                    GL.WriteLine($"Output to \"[This Program Path]\\OARBaseData\\{modName}{GL.FileExtension}\"");

                }
            }

            GL.WriteLine("Finished Reading.");
            GL.WriteLine("Please copy any csv files for the mod you want to \"[This Program Path]\\OAROverrideData\".");
            GL.WriteLine("Then add the new priority values.");
            GL.WriteLine("Then run OARPriorityRemapper.exe.");
            GL.WriteLine("Press Enter to close...");
            Console.ReadLine();

            Process.Start("explorer.exe", "OARBaseData");
        }

        

        
    }
}