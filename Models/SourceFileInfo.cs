using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gnyang.mcp.projectAutomation.Models
{
    internal class SourceFileInfo
    {
        public string? FolderName { get; set; }
        public string? Name { get; set; }
        public string? Extension { get; set; } = ".cs";
        public string? Contents { get; set; }
        public string? FullName
        {
            get
            {
                return Name + Extension;
            }
        }
    }
}
