using PersistAssist.Models;

namespace PersistAssist.Modules.Payloads
{
    class MsgBox : Payload
    {
        public override string PayloadName => "MsgBox";

        public override string PayloadDesc => "Displays a MessageBox";
        public override Data.Enums.PayloadLang PayloadLanguage => Data.Enums.PayloadLang.CSharp;
        public override string[] PayloadNamespaces => new string[] { "System", "System.Runtime.InteropServices" };
        public override string DLLImports => @"
        [DllImport(""user32.dll"", CharSet = CharSet.Auto)]
        public static extern int MessageBox(IntPtr hWnd, String text, String caption, int options);
        ";
        public override string PayloadCode => @"
            MessageBox(IntPtr.Zero, ""This message box was opened leveraging the winAPI from within C#, pretty cool huh?"", ""Cool Message Box"", 4);
        ";
    }
}
