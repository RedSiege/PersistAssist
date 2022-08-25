using PersistAssist.Models;

using static PersistAssist.Models.Data.Enums;

namespace PersistAssist.Functions
{
    class PSProfiles : Persist
    {
        public override string PersistName => "PSProfile";

        public override string PersistDesc => "Backdoors PowerShell profile files";

        public override string PersistUsage => "PeristAssist.exe -t PSProfile -c <command>";

        public override PersistType PersistCategory => PersistType.Misc;

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