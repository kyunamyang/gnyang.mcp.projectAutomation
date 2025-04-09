using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gnyang.mcp.projectAutomation.Models
{
    public class @Column
    {
        public string? TABLE_CATALOG { get; set; }
        public string? TABLE_SCHEMA { get; set; }
        public string? TABLE_NAME { get; set; }
        public int ORDINAL_POSITION { get; set; }
        public string? COLUMN_NAME { get; set; }
        public string? COLUMN_DEFAULT { get; set; }
        public string? IS_NULLABLE { get; set; }
        public string? DATA_TYPE { get; set; }
        public string? CHARACTER_SET_NAME { get; set; }
        public string? CHARACTER_MAXIMUM_LENGTH { get; set; }
        public string? NUMERIC_PRECISION { get; set; }
        public string? DATETIME_PRECISION { get; set; }
    }
}
