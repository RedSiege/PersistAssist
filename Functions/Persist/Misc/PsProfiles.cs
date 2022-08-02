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
            throw new System.NotImplementedException();
        }

        public override string PersistExec(ParsedArgs pArgs)
        {
            throw new System.NotImplementedException();
        }
    }
}