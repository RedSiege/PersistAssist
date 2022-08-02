using PersistAssist.Utils;
using PersistAssist.Models;

using static PersistAssist.Utils.TimeStomp;

namespace PersistAssist.Functions
{
    public class Timestomp : Tradecraft
    {
        public override string TradecraftName => "TimeStomp";

        public override string TradecraftDesc => "Modifies file and directory time stamps. Does not modify Entry timestamp";

        public override string TradecraftUsage => "PersistAssist.exe -t timestomp -a [view/modify/duplicate] -fp [file path] -df [duplicate file] " +
            "-ts <M/A/C/ALL> -nt <new time>";

        public override string TradecraftTask(ParsedArgs pArgs)
        {
            if (pArgs.Action.isEmpty() || pArgs.filePath.isEmpty() /*|| pArgs.Timestamp.isEmpty() || pArgs.newTime.isEmpty()*/) {
                throw new PersistAssistException("[-] Incorrect parameters passed. See technique usage");
            }

            if (pArgs.Action == "view") {
                return $"{ReturnObjTime(pArgs.filePath)}";
            } else if (pArgs.Action == "modify") {
                if (pArgs.Timestamp == "M") {
                    StompLastModifiedDate(pArgs.filePath, pArgs.newTime);
                } else if (pArgs.Timestamp == "A") {
                    StompLastAccessedDate(pArgs.filePath, pArgs.newTime);
                } else if (pArgs.Timestamp == "C") {
                    StompCreationDate(pArgs.filePath, pArgs.newTime);
                } else if (pArgs.Timestamp == "ALL") {
                    StompAll(pArgs.filePath, pArgs.newTime);
                } else { throw new PersistAssistException("[-] Incorrect timestamp parameter passed. See technique usage"); }
            } else if (pArgs.Action == "duplicate") {
                StompFromDupObjFileTime(pArgs.filePath, pArgs.dupPath);
            } else { throw new PersistAssistException("[-] Incorrect action parameter passed. See technique usage"); }

            return "";
        }
    }
}
