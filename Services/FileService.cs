using gnyang.mcp.projectAutomation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gnyang.mcp.projectAutomation.Services
{
    internal class FileService
    {
        public async Task SaveSouceFile(string targetPath, SourceFileInfo s)
        {
            try
            {
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                string fullPath = Path.Combine(targetPath, s.FullName!);
                await File.WriteAllTextAsync(fullPath, s.Contents);
            }
            catch (Exception ex)
            {
                throw new IOException("Error while saving the file ", ex);
            }
        }
    }
}