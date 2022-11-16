using System;
using System.Text;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using Microsoft.CSharp;

using PersistAssist.Models;

namespace PersistAssist.Utils
{
    class Compiler
    {
        public static string Compile(Payload payload)
        {

            StringBuilder payloadNamespaces = new StringBuilder();

            string payloadTemplate = @"
[NAMESPACES]
namespace [PAYLOADNAME] {
    class [PAYLOADNAME] {
        [DLLIMPORTS]
        public static void Main(string[] args) {
            [CODE]
        }
    }
}
            ";

            foreach (string _namespace in payload.PayloadNamespaces) { payloadNamespaces.AppendLine($"using {_namespace};"); }

            string craftedPayload = payloadTemplate.Replace("[PAYLOADNAME]", payload.PayloadName);
            craftedPayload = craftedPayload.Replace("[CODE]", payload.PayloadCode);
            craftedPayload = craftedPayload.Replace("[NAMESPACES]", payloadNamespaces.ToString());

            if (!payload.DLLImports.isEmpty()) { 
                craftedPayload = craftedPayload.Replace("[DLLIMPORTS]", payload.DLLImports); 
            } else { craftedPayload = craftedPayload.Replace("[DLLIMPORTS]", ""); }

            //Console.WriteLine(craftedPayload);

            Dictionary<string, string> providerOptions = new Dictionary<string, string> {
                    {"CompilerVersion", "v3.5"}
                };

            CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

            CompilerParameters compilerParams = new CompilerParameters
            {
                GenerateInMemory = false,
                GenerateExecutable = true,
                TreatWarningsAsErrors = false,
                OutputAssembly = $"{payload.PayloadName}.exe",
            };

            CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, craftedPayload);

            if (results.Errors.Count != 0) {
                StringBuilder errors = new StringBuilder();

                errors.AppendLine("Error(s):");
                foreach (CompilerError error in results.Errors) { errors.AppendLine($"- {error}"); }
                return errors.ToString();
                //throw new Exception("Mission failed! We'll get em next time bois"); 
            }

            return $"Compiled {payload.PayloadName} to {Environment.CurrentDirectory}\\{payload.PayloadName}.exe";

        }

    }
}
