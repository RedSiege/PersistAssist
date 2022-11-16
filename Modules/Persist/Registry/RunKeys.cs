using PersistAssist.Utils;
using PersistAssist.Models;

using static PersistAssist.Models.Data.Enums;

namespace PersistAssist.Functions
{
    class RunKeys : Persist
    {
        public override string PersistName => "RunKeys";
        public override string PersistDesc => "Registers a RunKey on either HKLM or HKCU";
        public override string PersistUsage => "\n\tPersist: PersistAssist.exe -t RunKeys -p -sk [sub key] -kv [value] -rc [hkcu/hklm]\n" +
            "\tCleanup: PeristAssist.exe -t RegKeys -c -sk [subkey] -rc [hkcu/hklm]";

        public override PersistType PersistCategory => PersistType.Registry;

        /*
        HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run
        HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\RunOnce
        HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\RunServices
        HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\RunServicesOnce
        HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Run
        HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\RunOnce
        HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\RunServices
        HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\RunServicesOnce
        HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnceEx\0001
        HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnceEx\0001\Depend
         */

        public override string PersistExec(ParsedArgs pArgs) {
            try { 
                if(pArgs.regSubKey.isEmpty() || pArgs.regKeyValue.isEmpty() || pArgs.regContext.isEmpty()) { 
                    throw new PersistAssistException("Incorrect parameters passed. See technique usage"); 
                }

                if (pArgs.regContext.Contains("hkcu")) {
                    return RegistryOps.AddKey(@"Software\Microsoft\Windows\CurrentVersion", pArgs.regSubKey, pArgs.regKeyValue, RegistryContext.HKCU);
                } else if (pArgs.regContext.Contains("hklm")) {
                    return RegistryOps.AddKey(@"Software\Microsoft\Windows\CurrentVersion", pArgs.regSubKey, pArgs.regKeyValue, RegistryContext.HKLM);
                } else { throw new PersistAssistException("Invalid registry context"); }
            } catch (System.Exception e) { return $"PersistAssist module failed: {e.Message}"; }
        }

        public override string PersistCleanup(ParsedArgs pArgs) {
            try {
                if (pArgs.regSubKey.isEmpty() || pArgs.regContext.isEmpty()) { throw new PersistAssistException("Incorrect parameters passed. See technique usage"); }

                if (pArgs.regContext.Contains("hkcu")) {
                    return RegistryOps.RemoveKey(@"Software\Microsoft\Windows\CurrentVersion", pArgs.regSubKey, RegistryContext.HKCU);
                } else if (pArgs.regContext.Contains("hklm")) {
                    return RegistryOps.RemoveKey(@"Software\Microsoft\Windows\CurrentVersion", pArgs.regSubKey, RegistryContext.HKLM);
                } else { throw new PersistAssistException("Invalid registry context"); }
            } catch (System.Exception e) { return $"PeristAssist module failed: {e.Message}"; }
        }
    }
}
