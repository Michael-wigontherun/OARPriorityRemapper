using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OARPriorityRemapper
{
    public class SubFolderData
    {
        public SubFolderData(string path, int orgPriority, int newPriority)
        {
            Path = path;
            OrgPriority = orgPriority;
            NewPriority = newPriority;
        }

        public string Path { get; set; } = "";
        public int OrgPriority { get; set; } = int.MinValue;
        public int NewPriority { get; set; } = int.MinValue;
    }
}
