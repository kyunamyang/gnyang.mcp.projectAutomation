
using gnyang.mcp.projectAutomation.Db;

namespace gnyang.mcp.projectAutomation
{
    internal class ConsoleProgram
    {
        async Task Main(string[] args)
        {
            string defaultDataSource = "localhost";
            string defaultInitialCatalog = "gnyang.company";
            string defaultUserId = "sa";
            string defaultPassword = "password";
            string defaultNameSpace = "Company.Project";
            string defaultTargetFolderName = "C:\\Project.output";

            Console.WriteLine("Enter the connection string of the DB to be processed.");

            string dataSource = GetInput("Data Source", defaultDataSource);
            string initialCatalog = GetInput("Initial Catalog", defaultInitialCatalog);
            string userId = GetInput("User ID", defaultUserId);
            string password = GetInput("Password", defaultPassword);
            string @nameSpace = GetInput("Default namespace", defaultNameSpace);
            string targetFolderName = GetInput("Target folder name", defaultTargetFolderName);

            string connectionString = $"Server={dataSource}; Database={initialCatalog}; User ID={userId};Password={password};TrustServerCertificate=true;";

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Processing ...");
            Console.ResetColor();

            DbReader reader = new DbReader(connectionString, @nameSpace, targetFolderName);
            await reader.Read();
        }

        string GetInput(string prompt, string defaultValue)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{prompt} (default: {defaultValue})");
            Console.ResetColor();

            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                input = defaultValue;
                Console.WriteLine($"{input}");
            }

            return input;
        }
    }
}
