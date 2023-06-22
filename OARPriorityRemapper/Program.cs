using OARPriorityGlobalLibrary;
using System.Text.Json;

namespace OARPriorityRemapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            if (GL.Close(args, "OARPriorityRemapper.txt")) return;

            Directory.CreateDirectory("OAROverrideData");

            string dataFolder = args[0];

            IEnumerable<string> subDatasFolders = Directory.GetFiles("OAROverrideData", $"*{GL.FileExtension}", SearchOption.TopDirectoryOnly);

            foreach(string subDatasFolder in subDatasFolders)
            {
                GL.WriteLine($"Reading \"OAROverrideData\\{Path.GetFileName(subDatasFolder)}\"");
                string[] subDataLines = File.ReadAllLines(subDatasFolder);
                if(subDataLines.Length < 2)
                {
                    Console.WriteLine($"{Path.GetFileName(subDatasFolder)} does not contain enough data.");
                    Console.WriteLine("The first line should be the sub path from the data folder to the OAR mod.");
                    continue;
                }

                string oarModFolderPath = Path.Combine(dataFolder, subDataLines[0].Split(";")[0]);

                List<SubFolderData> subFolders = new List<SubFolderData>();

                bool notUpdated = true;
                GL.WriteLine($"Checking for accurate data for {Path.GetFileName(subDatasFolder)}.");
                for(int i = 1; i < subDataLines.Length; i++)
                {
                    string[] lineArr = subDataLines[i].Split(";");

                    if(lineArr.Length < 3)
                    {
                        GL.WriteLine("Missing data on line " + (i + 1));
                        notUpdated = false;
                        break;
                    }

                    if (!Int32.TryParse(lineArr[1], out int orgPriority))
                    {
                        GL.WriteLine("Original priority not int on line " + (i + 1));
                        notUpdated = false;
                        break;
                    }

                    if (!Int32.TryParse(lineArr[2], out int newPriority))
                    {
                        GL.WriteLine("New priority not int on line " + (i + 1));
                        notUpdated = false;
                        break;
                    }

                    SubFolderData subFolderData = new SubFolderData(Path.Combine(oarModFolderPath, lineArr[0], "config.json"), orgPriority, newPriority);

                    if (!File.Exists(subFolderData.Path))
                    {
                        GL.WriteLine("Could not locate config.json file with line " + (i + 1));
                        notUpdated = false;
                        break;
                    }

                    using (JsonDocument document = JsonDocument.Parse(File.ReadAllText(subFolderData.Path)))
                    {
                        JsonElement root = document.RootElement;
                        if (root.TryGetProperty("priority", out JsonElement priority))
                        {
                            if (priority.ValueKind.Equals(JsonValueKind.Number))
                            {
                                if (priority.GetInt32() != orgPriority)
                                {
                                    GL.WriteLine($"Priority inside {lineArr[0]} config does not match the Original.");
                                    GL.WriteLine($"Ignore if you have run it over the mod before.");
                                    notUpdated = false;
                                    break;
                                }
                            }
                            else
                            {
                                GL.WriteLine($"Priority inside {lineArr[0]} config is not an int.");
                                notUpdated = false;
                                break;
                            }
                        }
                        else
                        {
                            GL.WriteLine($"{lineArr[0]} config does not contain priority.");
                            notUpdated = false;
                            break;
                        }
                    }

                    subFolders.Add(subFolderData);

                }//end file line reader




                if (!notUpdated) continue;




                GL.WriteLine("Starting output.");
                foreach (var subFolder in subFolders)
                {
                    string[] configLines = File.ReadAllLines(subFolder.Path);

                    for(int i = 0; i < configLines.Length; i++)
                    {
                        if (configLines[i].Contains("\"priority\""))
                        {
                            configLines[i] = configLines[i].Replace(subFolder.OrgPriority.ToString(), subFolder.NewPriority.ToString());
                            break;
                        }
                    }
                    GL.WriteLine(subFolder.Path);
                    File.WriteAllLines(subFolder.Path, configLines);
                }

            }//End OAROverride Folder reader

            GL.WriteLine("Press Enter to close...");
            Console.ReadLine();
        }

    }
}