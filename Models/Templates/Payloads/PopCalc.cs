namespace PersistAssist.Models.Templates
{
    public class PopCalc : Payload
    {
        public override string PayloadName => "PopCalc";

        public override string PayloadDesc => "Pops calc";
        public override string[] PayloadNamespaces => new string[] { "System", "System.Diagnostics" };

        public override string PayloadCode => @"
            Process.Start(""calc.exe"");
        ";
    }
}
