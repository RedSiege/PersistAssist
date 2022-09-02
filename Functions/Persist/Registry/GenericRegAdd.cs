using PersistAssist.Utils;
using PersistAssist.Models;

using static PersistAssist.Utils.Extensions;
using static PersistAssist.Models.Data.Enums;

namespace PersistAssist.Functions
{
    public class GenericRegAdd : Persist
    {
        public override string PersistName => "GenericRegAdd";

        public override string PersistDesc => "Add any arbitrary registry key";

        public override string PersistUsage => "\n\tPersist: PersistAssist.exe -t GenericRegAdd -p -rk [rootkey] -sk [subkey] -kv [key value] -rc [reg context]\n" +
            "\tCleanup: PersistAssist.exe -t GenericRegAdd -c -rk [root key] -sk [sub key] -rc [reg context]";

        public override PersistType PersistCategory => PersistType.Registry;

        public override string PersistExec(ParsedArgs pArgs) {
            try { 
                if (pArgs.regRootKey.isEmpty() || pArgs.regSubKey.isEmpty() || pArgs.regKeyValue.isEmpty() || pArgs.regContext.isEmpty()) { 
                    throw new PersistAssistException("[-] Incorrect parameters passed. See technique usage"); 
                }

                if (pArgs.regContext.Contains("hkcu")) {
                    return RegistryOps.RemoveKey(pArgs.regRootKey, pArgs.regSubKey, RegistryContext.HKCU);
                } else if (pArgs.regContext.Contains("hklm")) {
                    return RegistryOps.RemoveKey(pArgs.regRootKey, pArgs.regSubKey, RegistryContext.HKLM); 
                } else { throw new PersistAssistException("[-] Invalid registry context"); }

            } catch (System.Exception e) { return $"PersistAssist module failed: {e.Message}"; }
        }

        public override string PersistCleanup(ParsedArgs pArgs) {
            try { 
                if (pArgs.regRootKey.isEmpty() || pArgs.regSubKey.isEmpty() || pArgs.regContext.isEmpty()) { 
                    throw new PersistAssistException("[-] Incorrect parameters passed. See technique usage"); 
                }

                if (pArgs.regContext.Contains("hkcu")) {
                    return RegistryOps.RemoveKey(pArgs.regRootKey, pArgs.regSubKey, RegistryContext.HKCU);
                } else if (pArgs.regContext.Contains("hklm")) { 
                    return RegistryOps.RemoveKey(pArgs.regRootKey, pArgs.regSubKey, RegistryContext.HKLM); 
                } else { throw new PersistAssistException("[-] Invalid registry context"); }
            } catch (System.Exception e) { return $"PersistAssist module failed: {e.Message}"; }
        }
    }
}
