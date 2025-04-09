using gnyang.mcp.projectAutomation.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace gnyang.mcp.projectAutomation.ViewModels
{
    public class TableViewModel
    {
        public TableViewModel(@Table model)
        {
            Model = model ?? new @Table();
        }
        
        public ObservableCollection<ColumnViewModel> ColumnViewModelList { get; } = new ObservableCollection<ColumnViewModel>();

        private @Table _model;
        
        public @Table Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value ?? throw new ArgumentNullException(nameof(value), "Model cannot be null");
                }
            }
        }

        public bool IsModified { get; set; }

        public int SEQ
        {
            get => Model.SEQ;
            set
            {
                if (value != Model.SEQ)
                {
                    Model.SEQ = value;
                    IsModified = true;
                }
            }
        }

        public bool IS_CHECK
        {
            get => Model.IS_CHECK;
            set
            {
                if (value != Model.IS_CHECK)
                {
                    Model.IS_CHECK = value;
                    IsModified = true;
                }
            }
        }

        public string TABLE_CATALOG
        {
            get => Model?.TABLE_CATALOG ?? "";
            set
            {
                if (value != Model.TABLE_CATALOG)
                {
                    Model.TABLE_CATALOG = value;
                    IsModified = true;
                }
            }
        }
        
        public string TABLE_SCHEMA
        {
            get => Model?.TABLE_SCHEMA ?? "";
            set
            {
                if (value != Model.TABLE_SCHEMA)
                {
                    Model.TABLE_SCHEMA = value;
                    IsModified = true;
                }
            }
        }
        
        public string TABLE_NAME
        {
            get => Model?.TABLE_NAME ?? "";
            set
            {
                if (value != Model.TABLE_NAME)
                {
                    Model.TABLE_NAME = value;
                    IsModified = true;
                }
            }
        }
        
        public string TABLE_TYPE
        {
            get => Model?.TABLE_TYPE ?? "";
            set
            {
                if (value != Model.TABLE_TYPE)
                {
                    Model.TABLE_TYPE = value;
                    IsModified = true;
                }
            }
        }
    }
}