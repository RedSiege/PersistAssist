using PersistAssist.Models;

namespace PersistAssist.Modules.Payloads
{
    class PopCalc : Payload
    {
        public override string PayloadName => "PopCalc";

        public override string PayloadDesc => "pops calc";

        public override Data.Enums.PayloadLang PayloadLanguage => Data.Enums.PayloadLang.CSharp;

        public override string[] PayloadNamespaces => new string[] { "System", "System.Diagnostics" };

        public override string PayloadCode => @"
            Process.Start(""calc.exe"");
        ";
    }
}