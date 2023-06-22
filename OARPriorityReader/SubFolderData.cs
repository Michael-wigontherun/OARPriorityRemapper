using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OARPriorityReader
{
    public class SubFolderData
    {
        public SubFolderData(int priority, string name)
        {
            Priority = priority;
            Name = name;
        }

        public int Priority { get; set; }
        public string Name { get; set; }
    }
}
