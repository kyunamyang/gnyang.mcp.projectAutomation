using gnyang.mcp.projectAutomation.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gnyang.mcp.projectAutomation.ViewModels
{
    public class ColumnViewModel
    {
        public ColumnViewModel(@Column? model = null)
        {
            Model = model ?? new @Column();
        }

        private @Column? _model;
        public @Column? Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value;
                }
            }
        }


        public bool IsModified { get; set; }

        public string TABLE_CATALOG
        {
            get => Model?.TABLE_CATALOG ?? "";
            set
            {
                if (value != Model!.TABLE_CATALOG)
                {
                    Model!.TABLE_CATALOG = value;
                    IsModified = true;
                }
            }
        }

        public string TABLE_SCHEMA
        {
            get => Model?.TABLE_SCHEMA ?? "";
            set
            {
                if (value != Model!.TABLE_SCHEMA)
                {
                    Model!.TABLE_SCHEMA = value;
                    IsModified = true;
                }
            }
        }

        public string TABLE_NAME
        {
            get => Model?.TABLE_NAME ?? "";
            set
            {
                if (value != Model?.TABLE_NAME)
                {
                    Model!.TABLE_NAME = value;
                    IsModified = true;
                }
            }
        }
        public int ORDINAL_POSITION
        {
            get => Model!.ORDINAL_POSITION;
            set
            {
                if (value != Model!.ORDINAL_POSITION)
                {
                    Model.ORDINAL_POSITION = value;
                    IsModified = true;
                }
            }
        }
        public string COLUMN_NAME
        {
            get => Model?.COLUMN_NAME ?? "";
            set
            {
                if (value != Model!.COLUMN_NAME)
                {
                    Model!.COLUMN_NAME = value;
                    IsModified = true;
                }
            }
        }
        public string COLUMN_DEFAULT
        {
            get => Model?.COLUMN_DEFAULT ?? "";
            set
            {
                if (value != Model!.COLUMN_DEFAULT)
                {
                    Model!.COLUMN_DEFAULT = value;
                    IsModified = true;
                }
            }
        }

        public string IS_NULLABLE
        {
            get => Model?.IS_NULLABLE ?? "";
            set
            {
                if (value != Model!.IS_NULLABLE)
                {
                    Model.IS_NULLABLE = value;
                    IsModified = true;
                }
            }
        }

        public string DATA_TYPE
        {
            get => Model?.DATA_TYPE ?? "";
            set
            {
                if (value != Model!.DATA_TYPE)
                {
                    Model.DATA_TYPE = value;
                    IsModified = true;
                }
            }
        }

        public string CHARACTER_SET_NAME
        {
            get => Model?.CHARACTER_SET_NAME ?? "";
            set
            {
                if (value != Model!.CHARACTER_SET_NAME)
                {
                    Model.CHARACTER_SET_NAME = value;
                    IsModified = true;
                }
            }
        }

        public string CHARACTER_MAXIMUM_LENGTH
        {
            get => Model?.CHARACTER_MAXIMUM_LENGTH ?? "";
            set
            {
                if (value != Model!.CHARACTER_MAXIMUM_LENGTH)
                {
                    Model.CHARACTER_MAXIMUM_LENGTH = value;
                    IsModified = true;
                }
            }
        }

        public string NUMERIC_PRECISION
        {
            get => Model?.NUMERIC_PRECISION ?? "";
            set
            {
                if (value != Model!.NUMERIC_PRECISION)
                {
                    Model.NUMERIC_PRECISION = value;
                    IsModified = true;
                }
            }
        }

        public string DATETIME_PRECISION
        {
            get => Model?.DATETIME_PRECISION ?? "";
            set
            {
                if (value != Model!.DATETIME_PRECISION)
                {
                    Model.DATETIME_PRECISION = value;
                    IsModified = true;
                }
            }
        }
    }
}
