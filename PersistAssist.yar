rule PersistAssist_Yara {
    meta:
        last_updated = "2022-8-3"
        author = "Grimmie"
        description = "Searches for various strings present in an unmodified version of PersistAssist"

    strings:
        //namespaces used
        $MSNamespace1 = "Microsoft.Win32" nocase ascii
        $SysNamespace1 = "System.IO" nocase ascii
        $SysNamespace2 = "System.Collections.Generic" nocase ascii
        $SysNamespace3 = "System.Core" nocase ascii
        $SysNamespace4 = "System.Runtime.Versioning" nocase ascii
        $SysNamespace5 = "System.Security.Principal" nocase ascii
        $SysNamespace6 = "System.Reflection" nocase ascii
        $SysNamespace7 = "System.Linq" nocase ascii
        $SysNamespace8 = "System.Diagnostics" nocase ascii
        $SysNamespace9 = "System.Runtime.InteropServices" nocase ascii
        $SysNamespace10 = "System.Runtime.CompilerServices" nocase ascii

        $PANamespace1 = "PersistAsssist.Models" nocase ascii
        $PANamespace1 = "PersistAsssist.Utils" nocase ascii
        $PANamespace1 = "PersistAsssist.Functions" nocase ascii

        //APIs referenced 
        $CredAPICalls1 = "CredReadA" nocase ascii
        $CredAPICalls2 = "CredEnumerateA" nocase ascii
        $CredAPICalls3 = "CredWriteA" nocase ascii
        $CredAPICalls4 = "CredFree" nocase ascii

        $TSAPICalls1 = "GetFileTime" nocase ascii
        $TSAPICalls2 = "SetFileTime" nocase ascii

        $GenAPI1 = "GetLastError" nocase ascii

        //structs
        $TSStructs1 = "VALIDTIME" nocase ascii
        $TSStructs2 = "FILETIME" nocase ascii
        $TSStructs3 = "ALLDATETIME" nocase ascii

        //misc strings
        $regStr1 = "HKCU" nocase ascii
        $regStr2 = "HKLM" nocase ascii

        $persist1 = "PersistExec" nocase ascii
        $persist2 = "FromFileTimeUtc" nocase ascii

        $DInvoke1 = "DynmaicFunctionInvoke" nocase ascii
        $DInvoke2 = "DynmaicInvoke" nocase ascii
        $DInvoke3 = "GetDelegateForFunctionPointer" nocase ascii

        $TSMethod1 = "StompFromDupObjFileTime" nocase ascii
        $TSMethod2 = "ReturnObjTime" nocase ascii
        $TSMethod3 = "VerifyInputTime" nocase ascii

        $CredStr1 = "credType" nocase ascii
        $CredStr2 = "CredentialType" nocase ascii
        $CredStr3 = "CredentialBlogSize" nocase ascii

    condition:
        // searches all dynamically invoked API, these strings can't be changed
        (all of $CredAPICalls* and
        all of $TSAPICalls* and
        all of $GenAPI*) and
        // searches for "HCKU" and "HLKM" (these can't be changed) and timestomping structs
        all of $regStr* and
        1 of $TSStructs* or
        // checks if any one method name is unchanged
        (1 of $DInvoke* or
        1 of $TSMethod* or
        1 of $persist* or 
        1 of $TSMethod* or 
        1 of $CredStr*) or
        // searches for namespaces
        (1 of $MSNamespace* and
        1 of $SysNamespace* and
        1 of $PANamespace*)

}
