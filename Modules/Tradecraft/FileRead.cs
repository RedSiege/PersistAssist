using System.IO;
using System.Text;

using PersistAssist.Utils;
using PersistAssist.Models;

namespace PersistAssist.Functions
{
    class FileRead : Tradecraft
    {
        public override string TradecraftName => "FileRead";

        public override string TradecraftDesc => "Reads a file in memory to get around having to download files for reading";

        public override string TradecraftUsage => "PersistAssist.exe -t FileRead -fp [file path]";

        public override string TradecraftTask(ParsedArgs pArgs)
        {
            if (pArgs.filePath.isEmpty()) { throw new PersistAssistException("Incorrect parameters passed. See technique usage"); }

            StringBuilder fileOut = new StringBuilder();

            string[] fileContents = File.ReadAllLines(pArgs.filePath);

            foreach(string line in fileContents) { fileOut.AppendLine(line); }

            return fileOut.ToString();
        }
    }
}
