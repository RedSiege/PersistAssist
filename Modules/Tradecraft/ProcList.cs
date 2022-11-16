using PersistAssist.Utils;
using PersistAssist.Models;

using static PersistAssist.Utils.Procs;

namespace PersistAssist.Functions
{
    class ProcList : Tradecraft
    {
        public override string TradecraftName => "ProcList";

        public override string TradecraftDesc => "Lists running processes";

        public override string TradecraftUsage => "PeristAssist.exe -t ProcList -s [process name]";

        public override string TradecraftTask(ParsedArgs pArgs) {
            if (!pArgs.Search.isEmpty()) {
                return listProcs(pArgs.Search);
            } else { return listProcs(); }
        }
    }
}
