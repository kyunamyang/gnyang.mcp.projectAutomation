using gnyang.mcp.projectAutomation.Db;
using ModelContextProtocol.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gnyang.mcp.projectAutomation
{
    [McpServerToolType]
    public static class EntityTools
    {
        [McpServerTool, Description("Generate entity files from mssql")]
        public static async Task SaveEntities(
             [Description("dataSource")] string dataSource,
             [Description("initialCatalog")] string initialCatalog,
             [Description("userId")] string userId,
             [Description("password")] string password,
             [Description("namespace")] string _namespace,
             [Description("targetFolderName")] string targetFolderName
            )
        {
            string connectionString = $"Server={dataSource}; Database={initialCatalog}; User ID={userId};Password={password};TrustServerCertificate=true;";

            DbReader reader = new DbReader(connectionString, _namespace, targetFolderName);
            await reader.Read();
        }

        [McpServerTool, Description("Generate typescript interface files from mssql")]
        public static async Task SaveTypescriptInterfaces(
             [Description("dataSource")] string dataSource,
             [Description("initialCatalog")] string initialCatalog,
             [Description("userId")] string userId,
             [Description("password")] string password,
             [Description("namespace")] string _namespace,
             [Description("targetFolderName")] string targetFolderName
            )
        {
            string connectionString = $"Server={dataSource}; Database={initialCatalog}; User ID={userId};Password={password};TrustServerCertificate=true;";

            DbReader reader = new DbReader(connectionString, _namespace, targetFolderName);
            await reader.Read(true);
        }
    }
}
