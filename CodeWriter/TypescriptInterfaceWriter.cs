using gnyang.mcp.projectAutomation.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace gnyang.mcp.projectAutomation.CodeWriter
{
    internal class TypescriptInterfaceWriter
    {
        public string ConvertCSharpClassToTypeScriptInterface(string csharpClassCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(csharpClassCode);
            var root = syntaxTree.GetRoot() as CompilationUnitSyntax;

            var classDeclaration = root.DescendantNodes()
                                        .OfType<ClassDeclarationSyntax>()
                                        .FirstOrDefault();

            if (classDeclaration == null)
            {
                throw new InvalidOperationException("No class found in the provided C# code.");
            }

            var tsInterface = new StringBuilder();
            tsInterface.AppendLine($"export interface {classDeclaration.Identifier.Text} {{");

            foreach (var member in classDeclaration.Members)
            {
                if (member is PropertyDeclarationSyntax property)
                {
                    var tsPropertyName = ToCamelCase(property.Identifier.Text);
                    var tsPropertyType = CSharpToTypeScriptType(property.Type.ToString());
                    tsInterface.AppendLine($"    {tsPropertyName}: {tsPropertyType};");
                }
            }

            tsInterface.AppendLine("}");
            return tsInterface.ToString();
        }

        private string CSharpToTypeScriptType(string csharpType)
        {
            switch (csharpType)
            {
                case "int":
                case "long":
                case "double":
                case "float":
                    return "number";
                case "string":
                    return "string";
                case "bool":
                    return "boolean";
                case "DateTime":
                    return "Date";
                default:
                    return "any"; // 기본적으로 unknown 타입은 'any'
            }
        }

        private string ToCamelCase(string str)
        {
            return char.ToLower(str[0]) + str.Substring(1);
        }
    }
}
