using System.Security.Principal;

namespace PersistAssist.Utils
{
    static  class Extensions
    {
        public static bool isEmpty(this string str) { return string.IsNullOrEmpty(str); }

        public static bool isAdmin()
        {
            WindowsIdentity cID = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(cID);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static string toTitle(this string str) { return str[0].ToString().ToUpper() + str.Substring(1); }


        public static string Align(this object obj, int length) { return obj.ToString().PadRight(length); }
    }
}
