using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gnyang.mcp.projectAutomation.Models
{
    internal class IdentityColumn
    {
        public string? schema { get; set; }
        public string? tableName { get; set; }
        public int column_id { get; set; }
        public string? name { get; set; }
    }

}