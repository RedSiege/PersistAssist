using PersistAssist.Models;

namespace PersistAssist.Modules.Payloads
{
    class PopCalcAPI : Payload
    {
        public override string PayloadName => "PopCalcAPI";

        public override string PayloadDesc => "Pops calc via the API";

        public override Data.Enums.PayloadLang PayloadLanguage => Data.Enums.PayloadLang.CSharp;
        public override string[] PayloadNamespaces => new string[] { "System", "System.Runtime.InteropServices" };

        public override string DLLImports => @"
        [DllImport(""Shell32.dll"", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);
        ";

        public override string PayloadCode => @"
            ShellExecute(IntPtr.Zero, ""open"", ""C:\\Windows\\SysWOW64\\calc.exe"", null, null, 1);
        ";
    }
}