using PersistAssist.Models;

using static PersistAssist.Models.Data.Enums;

namespace PersistAssist.Functions
{    
    internal class StartupFolder : Persist
    {
        public override string PersistName => "StartupFolder";

        public override string PersistDesc => "Drops a shortcut to a startup path";

        public override string PersistUsage => "PersistAssist.exe -t StartupFolder";

        public override PersistType PersistCategory => PersistType.Misc;

        public static string startupPath = "";

        public override string PersistCleanup(ParsedArgs pArgs)
        {
            try {
                throw new System.NotImplementedException();
            } catch (System.Exception e) { return $"PersistAssist module failed: {e.Message}"; }
        }

        public override string PersistExec(ParsedArgs pArgs)
        {
            try {
                throw new System.NotImplementedException();
            } catch (System.Exception e) { return $"PersistAssist module failed: {e.Message}"; }
        }
    }
}
