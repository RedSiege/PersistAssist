using System.Security.Principal;

namespace PersistAssist.Utils
{
    static class Extensions
    {
        public static bool isEmpty(this string str){ return string.IsNullOrEmpty(str); }

        public static bool isAdmin() {
            WindowsIdentity cID = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(cID);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
