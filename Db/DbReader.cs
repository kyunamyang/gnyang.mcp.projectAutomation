using gnyang.mcp.projectAutomation.ViewModels;
using gnyang.mcp.projectAutomation.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using gnyang.mcp.projectAutomation.CodeWriter;
using gnyang.mcp.projectAutomation.Services;

namespace gnyang.mcp.projectAutomation.Db
{
    internal class DbReader
    {

        private string ConnectionString { get; } = string.Empty;
        private string defaultEntityNamespace = "";
        private string targetFolderName = "bin";
        private List<SourceFileInfo> sourceFileInfoList = new List<SourceFileInfo>();

        public ObservableCollection<TableViewModel> TableViewModelList { get; } = new ObservableCollection<TableViewModel>();
        private ObservableCollection<ColumnViewModel> ColumnViewModelList { get; } = new ObservableCollection<ColumnViewModel>();
        private List<IdentityColumn> IdentityColumnList { get; set; } = new List<IdentityColumn>();
        private List<ComputedColumn> ComputedColumnList { get; set; } = new List<ComputedColumn>();
        private List<PrimaryKeyColumn> PrimaryKeyColumnList { get; set; } = new List<PrimaryKeyColumn>();

        public DbReader(string connectionString, string defaultEntityNamespace, string targetFolderName)
        {
            ConnectionString = connectionString;
            this.defaultEntityNamespace = defaultEntityNamespace;
            this.targetFolderName = targetFolderName;
        }

        public async Task Read(bool typescript = false)
        {

            var tables = GetTables();
            SetTableViewModelList(tables);

            var columns = GetColumns();
            SetColumnList(columns);

            var identityColumns = GetIdentityColumns(tables);
            IdentityColumnList = identityColumns; 

            var computedColumns = GetComputedColumns(tables);
            ComputedColumnList = computedColumns; 

            var primaryKeyColumns = GetKeyColumns(tables);
            PrimaryKeyColumnList = primaryKeyColumns; 

            SetTablesColumn();

            if (typescript == false)
                await GenerateEntity();
            else
                await GenerateTypescriptInterface();
        }

        private ObservableCollection<@Table> GetTables()
        {
            const string tableQuery = @"
SELECT	TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE
FROM	INFORMATION_SCHEMA.TABLES 
WHERE	TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME
";
            var result = new ObservableCollection<@Table>();
            //var connectResult = "Fail";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = tableQuery;
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                int seq = 1;
                                while (reader.Read())
                                {
                                    var t = new @Table();
                                    t.SEQ = seq++;
                                    t.IS_CHECK = true;
                                    t.TABLE_CATALOG = reader.GetString(0);
                                    t.TABLE_SCHEMA = reader.GetString(1);
                                    t.TABLE_NAME = reader.GetString(2);
                                    t.TABLE_TYPE = reader.GetString(3);

                                    result.Add(t);
                                }
                                //connectResult = "Success";
                            }
                        }
                    }
                    else
                    {
                        //connectResult = "Fail";
                    }
                    conn.Close();
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            finally
            {
            }

            return result;
        }

        private ObservableCollection<@Column> GetColumns()
        {

            const string columnQuery = @"
SELECT	TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, ORDINAL_POSITION, COLUMN_NAME
        , COLUMN_DEFAULT, IS_NULLABLE, DATA_TYPE, CHARACTER_SET_NAME
        , CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, DATETIME_PRECISION
FROM	INFORMATION_SCHEMA.COLUMNS
ORDER BY TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, ORDINAL_POSITION ASC
";
            var columns = new ObservableCollection<@Column>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = columnQuery;
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var c = new @Column();
                                    c.TABLE_CATALOG = reader.GetString(0);
                                    c.TABLE_SCHEMA = reader.GetString(1);
                                    c.TABLE_NAME = reader.GetString(2);
                                    c.ORDINAL_POSITION = reader.GetInt32(3);
                                    c.COLUMN_NAME = reader.GetString(4);
                                    c.COLUMN_DEFAULT = reader.IsDBNull(5) ? null : reader.GetString(5);
                                    c.IS_NULLABLE = reader.GetString(6);
                                    c.DATA_TYPE = reader.GetString(7);
                                    c.CHARACTER_SET_NAME = reader.IsDBNull(8) ? null : reader.GetString(8);
                                    c.CHARACTER_MAXIMUM_LENGTH = reader.IsDBNull(9) ? null : Convert.ToString(reader.GetInt32(9));
                                    c.NUMERIC_PRECISION = reader.IsDBNull(10) ? null : Convert.ToString(reader.GetByte(10));
                                    c.DATETIME_PRECISION = reader.IsDBNull(11) ? null : Convert.ToString(reader.GetInt16(11));

                                    columns.Add(c);
                                }
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return columns;
        }

        private List<IdentityColumn> GetIdentityColumns(ObservableCollection<@Table> tables)
        {
            string identityQuery = @"
SELECT	c.column_id, c.name
FROM	sys.columns c 
			INNER JOIN sys.identity_columns ic ON c.object_id = ic.object_id AND c.column_id = ic.column_id
WHERE	c.object_id = OBJECT_ID('{0}.{1}')
";
            var identityColumns = new List<IdentityColumn>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            foreach (var table in tables)
                            {
                                cmd.CommandText = string.Format(identityQuery, table.TABLE_SCHEMA, table.TABLE_NAME);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var c = new IdentityColumn();
                                        c.schema = table.TABLE_SCHEMA!;
                                        c.tableName = table.TABLE_NAME!;
                                        c.column_id = reader.GetInt32(0);
                                        c.name = reader.GetString(1);

                                        identityColumns.Add(c);
                                    }
                                }
                            }

                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return identityColumns;
        }

        private List<ComputedColumn> GetComputedColumns(ObservableCollection<@Table> tables)
        {

            string identityQuery = @"
SELECT	c.column_id, c.name
FROM	sys.columns c 
			INNER JOIN sys.computed_columns cc ON c.object_id = cc.object_id AND c.column_id = cc.column_id
WHERE	c.object_id = OBJECT_ID('{0}.{1}')
";
            var computedColumns = new List<ComputedColumn>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            foreach (var table in tables)
                            {
                                cmd.CommandText = string.Format(identityQuery, table.TABLE_SCHEMA, table.TABLE_NAME);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var c = new ComputedColumn();
                                        c.schema = table.TABLE_SCHEMA!;
                                        c.tableName = table.TABLE_NAME!;
                                        c.column_id = reader.GetInt32(0);
                                        c.name = reader.GetString(1);

                                        computedColumns.Add(c);
                                    }
                                }
                            }

                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return computedColumns;
        }

        private List<PrimaryKeyColumn> GetKeyColumns(ObservableCollection<@Table> tables)
        {
            string query = @"
    SELECT	TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, ORDINAL_POSITION, COLUMN_NAME
    FROM	INFORMATION_SCHEMA.KEY_COLUMN_USAGE
    WHERE	OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1
          AND TABLE_SCHEMA = '{0}'
          AND TABLE_NAME = '{1}' 
";
            var keyColumns = new List<PrimaryKeyColumn>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            foreach (var table in tables)
                            {
                                cmd.CommandText = string.Format(query, table.TABLE_SCHEMA, table.TABLE_NAME);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var c = new PrimaryKeyColumn();
                                        c.TABLE_CATALOG = reader.GetString(0);
                                        c.TABLE_SCHEMA = reader.GetString(1);
                                        c.TABLE_NAME = reader.GetString(2);
                                        c.ORDINAL_POSITION = reader.GetInt32(3);
                                        c.COLUMN_NAME = reader.GetString(4);

                                        keyColumns.Add(c);
                                    }
                                }
                            }

                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return keyColumns;
        }

        public void SetTableViewModelList(ObservableCollection<@Table> list)
        {

            TableViewModelList.Clear();

            foreach (var item in list)
            {
                TableViewModelList.Add(new TableViewModel(item));
            }
        }

        public void SetColumnList(ObservableCollection<@Column> columnList)
        {

            ColumnViewModelList.Clear();

            foreach (var item in columnList)
            {
                ColumnViewModelList.Add(new ColumnViewModel(item));
            }
        }

        public void SetTablesColumn()
        {
            foreach (var table in TableViewModelList)
            {
                var columns = ColumnViewModelList
                    .Where(el => el.TABLE_CATALOG == table.TABLE_CATALOG &&
                        el.TABLE_SCHEMA == table.TABLE_SCHEMA &&
                        el.TABLE_NAME == table.TABLE_NAME)
                    .OrderBy(el => el.ORDINAL_POSITION).ToList();

                foreach (var c in columns)
                {
                    table.ColumnViewModelList.Add(c);
                }
            }
        }

        private async Task GenerateEntity()
        {
            EntityWriter writer = new EntityWriter(defaultEntityNamespace);
            sourceFileInfoList = writer.WriteEntity(this);

            FileService fs = new FileService();

            string output = Directory.GetCurrentDirectory();
            output = Path.Combine(output, targetFolderName);

            foreach (var c in sourceFileInfoList)
            {
                await fs.SaveSouceFile(output, c);
            }
        }

        private async Task GenerateTypescriptInterface()
        {
            EntityWriter writer = new EntityWriter(defaultEntityNamespace);
            sourceFileInfoList = writer.WriteEntity(this);

            TypescriptInterfaceWriter tsInterfaceWriter = new TypescriptInterfaceWriter();
            foreach ( var s in sourceFileInfoList)
            {
                s.Extension = ".ts";
                s.Contents = tsInterfaceWriter.ConvertCSharpClassToTypeScriptInterface(s.Contents!);
            }

            FileService fs = new FileService();

            string output = Directory.GetCurrentDirectory();
            output = Path.Combine(output, targetFolderName);

            foreach (var c in sourceFileInfoList)
            {
                await fs.SaveSouceFile(output, c);
            }
        }
    }
}