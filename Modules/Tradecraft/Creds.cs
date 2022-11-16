using PersistAssist.Models;

using static PersistAssist.Utils.Creds;

namespace PersistAssist.Functions
{
    public class Creds : Tradecraft
    {
        public override string TradecraftName => "Creds";

        public override string TradecraftDesc => "Cred operations";

        public override string TradecraftUsage => "PersistAssist.exe -t Creds -a [check/list]\n" +
            "\tcheck: PersistAssist.exe -t Creds -a creds -un <username> -pw <passwd> -dn <domain name>\n" +
            "\tlist: PersistAssist.exe -t Creds -a list";

        public override string TradecraftTask(ParsedArgs pArgs)
        {
            if (pArgs.Action == "check")
            {
                return credCheck(pArgs.userName, pArgs.Passwd, pArgs.domain);
            }
            else if (pArgs.Action == "list")
            {
                return readCredData();
            }
            else { throw new PersistAssistException("Incorrect action parameter passed. See technique usage"); }
        }
    }
}
