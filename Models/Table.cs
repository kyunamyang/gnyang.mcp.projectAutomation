using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gnyang.mcp.projectAutomation.Models
{
    public class @Table
    {
        public int SEQ { get; set; }
        public bool IS_CHECK { get; set; }
        public string? TABLE_CATALOG { get; set; }
        public string? TABLE_SCHEMA { get; set; }
        public string? TABLE_NAME { get; set; }
        public string? TABLE_TYPE { get; set; }

    }
}
