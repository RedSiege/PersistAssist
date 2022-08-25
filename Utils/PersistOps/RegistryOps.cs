using System;

using Microsoft.Win32;

using static PersistAssist.Models.Data.Enums;

namespace PersistAssist.Utils
{
    public class RegistryOps
    {
        public static string AddKey(string rootkey, string subkey, string keyvalue, RegistryContext registryContext) {
            
            if (registryContext is RegistryContext.HKLM) {
                Registry.LocalMachine.CreateSubKey(rootkey).SetValue(subkey, keyvalue);
            } else {
                Registry.CurrentUser.CreateSubKey(rootkey).SetValue(subkey, keyvalue);
            }

            return $"[*] Value \"{ReadKey(rootkey, subkey, registryContext)}\" written to {registryContext}\\{rootkey}\\{subkey}";
        }

        public static string RemoveKey(string key, string value, RegistryContext registryContext) {
            try {
                if (registryContext is RegistryContext.HKLM) {
                    Registry.LocalMachine.OpenSubKey(key, true).DeleteValue(value, true);
                } else {
                    Registry.CurrentUser.OpenSubKey(key, true).DeleteValue(value, true);
                }
            } catch (Exception e) { return $"[*] {registryContext}\\{key} failed to be removed"; }      
           
           return $"[*] {registryContext}\\{key} with value \"{value}\" successfully removed";
        }

        public static string ReadKey(string rootkey, string subkey, RegistryContext registryContext) {
            string regOut;

            try {
                if (registryContext is RegistryContext.HKLM) {
                    regOut = Registry.LocalMachine.OpenSubKey(rootkey).GetValue(subkey, null).ToString();
                } else {
                    regOut = Registry.CurrentUser.OpenSubKey(rootkey).GetValue(subkey, null).ToString();
                }

                return regOut;
            } catch (Exception e) { return $"[*] {registryContext}\\{rootkey}\\{subkey} doesn't exist"; }
        }

        public static string ReadKey(string rootkey, RegistryContext registryContext) {

            if (registryContext is RegistryContext.HKLM) {
                return Registry.LocalMachine.OpenSubKey(rootkey).GetSubKeyNames().ToString();
            } else {
                return Registry.CurrentUser.OpenSubKey(rootkey).GetSubKeyNames().ToString();
            }
        }

    }
}
