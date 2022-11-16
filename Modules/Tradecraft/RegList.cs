using PersistAssist.Models;

using static PersistAssist.Utils.RegistryOps;

using static PersistAssist.Utils.Extensions;

namespace PersistAssist.Functions
{
    public class RegList : Tradecraft
    {
        public override string TradecraftName => "RegList";

        public override string TradecraftDesc => "Lists contents of specified registry key";

        public override string TradecraftUsage => "PersistAssist.exe -t RegList -rk [rootkey] -sk [subkey] -rc [hkcu/hklm]";

        public override string TradecraftTask(ParsedArgs pArgs)
        {
            try {
                if (pArgs.regRootKey.isEmpty() || pArgs.regSubKey.isEmpty()) { throw new PersistAssistException("Incorrect parameters passed. See technique usage"); }
                if (pArgs.regContext.Contains("hkcu")) {
                    return ReadKey(pArgs.regRootKey, pArgs.regSubKey, Data.Enums.RegistryContext.HKCU);
                } else if (pArgs.regContext.Contains("hklm")) {
                    return ReadKey(pArgs.regRootKey, pArgs.regSubKey, Data.Enums.RegistryContext.HKLM);
                } else { throw new PersistAssistException("Invalid registry context"); }

            } catch (System.Exception e) { return $"PersistAssist module failed: {e.Message}"; }
        }
    }
}
