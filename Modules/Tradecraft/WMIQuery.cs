using System;

using PersistAssist.Models;
using PersistAssist.Utils;

using static PersistAssist.Utils.WMIOps;

namespace PersistAssist.Functions
{
    public class WMIQuery : Tradecraft
    {
        public override string TradecraftName => "WMIQuery";

        public override string TradecraftDesc => "Run an arbitrary WMI Query";

        public override string TradecraftUsage => "PersistAssist.exe -t WMIQuery -q [query] -s <properties, comma separated>";

        public override string TradecraftTask(ParsedArgs pArgs)
        {
            try {
                if (!pArgs.Search.isEmpty())
                {
                    return Query(pArgs.Query, pArgs.Search.Split(','));
                }
                else { return Query(pArgs.Query); }
            } catch (Exception e) { throw new PersistAssistException(e.Message); }
        }
    }
}
