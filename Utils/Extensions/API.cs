using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using static PersistAssist.Utils.Structs;

namespace PersistAssist.Utils
{
    public class API {
        public class Delegates
        {
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void RtlInitUnicodeString(ref WinStructs.UNICODE_STRING DestinationString, [MarshalAs(UnmanagedType.LPWStr)] string SourceString);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate IntPtr CreateFile(String lpFileName, FileStructs.EFileAccess dwDesiredAccess, FileStructs.EFileShare dwShareMode,
                IntPtr securityAttrs, FileStructs.ECreationDisposition dwCreationDisposition, FileStructs.EFileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate Boolean GetFileTime(IntPtr hFile, ref FileStructs.FILETIME lpCreationTime, ref FileStructs.FILETIME lpLastAccessTime, 
                ref FileStructs.FILETIME lpLastWriteTime);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate Boolean SetFileTime(IntPtr hFile, ref long lpCreationTime, ref long lpLastAccessTime, ref long lpLastWriteTime);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate Boolean CloseHandle(IntPtr hObject);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate uint GetLastError();

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate Boolean CredReadA(string targetName, CredStructs.CredentialType credType, int reservedFlag, out IntPtr credPtr);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate Boolean CredWriteA(ref CredStructs.CredentialType credType, UInt32 flags);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate Boolean CredEnumerateA(string filter, int flag, out int count, out IntPtr creds);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate Boolean CredFree(IntPtr cred);
             
        }

        public class TSS
        {

            public static WinStructs.NTSTATUS LdrLoadDll(IntPtr PathToFile, UInt32 dwFlags, ref WinStructs.UNICODE_STRING ModuleFileName, ref IntPtr ModuleHandle)
            {
                object[] funcargs =
                {
                PathToFile, dwFlags, ModuleFileName, ModuleHandle
            };

                WinStructs.NTSTATUS retValue = (WinStructs.NTSTATUS)DynamicAPIInvoke(@"ntdll.dll", @"LdrLoadDll", typeof(Delegates.RtlInitUnicodeString), ref funcargs);

                ModuleHandle = (IntPtr)funcargs[3];

                return retValue;
            }

            public static void RtlInitUnicodeString(ref WinStructs.UNICODE_STRING DestinationString, [MarshalAs(UnmanagedType.LPWStr)] string SourceString)
            {
                object[] funcargs =
                {
                DestinationString, SourceString
            };

                DynamicAPIInvoke(@"ntdll.dll", @"RtlInitUnicodeString", typeof(Delegates.RtlInitUnicodeString), ref funcargs);

                DestinationString = (WinStructs.UNICODE_STRING)funcargs[0];
            }


            public static object DynamicAPIInvoke(string DLLName, string FunctionName, Type FunctionDelegateType, ref object[] Parameters)
            {
                IntPtr pFunction = GetLibraryAddress(DLLName, FunctionName);
                return DynamicFunctionInvoke(pFunction, FunctionDelegateType, ref Parameters);
            }


            public static object DynamicFunctionInvoke(IntPtr FunctionPointer, Type FunctionDelegateType, ref object[] Parameters)
            {
                Delegate funcDelegate = Marshal.GetDelegateForFunctionPointer(FunctionPointer, FunctionDelegateType);
                return funcDelegate.DynamicInvoke(Parameters);
            }

            public static IntPtr LoadModuleFromDisk(string DLLPath)
            {
                WinStructs.UNICODE_STRING uModuleName = new WinStructs.UNICODE_STRING();
                RtlInitUnicodeString(ref uModuleName, DLLPath);

                IntPtr hModule = IntPtr.Zero;
                WinStructs.NTSTATUS CallResult = LdrLoadDll(IntPtr.Zero, 0, ref uModuleName, ref hModule);
                if (CallResult != WinStructs.NTSTATUS.Success || hModule == IntPtr.Zero)
                {
                    return IntPtr.Zero;
                }

                return hModule;
            }


            public static IntPtr GetLoadedModuleAddress(string DLLName)
            {
                ProcessModuleCollection ProcModules = Process.GetCurrentProcess().Modules;
                foreach (ProcessModule Mod in ProcModules)
                {
                    if (Mod.FileName.ToLower().EndsWith(DLLName.ToLower()))
                    {
                        return Mod.BaseAddress;
                    }
                }
                return IntPtr.Zero;
            }

            public static IntPtr GetLibraryAddress(string DLLName, string FunctionName, bool CanLoadFromDisk = false)
            {
                IntPtr hModule = GetLoadedModuleAddress(DLLName);
                if (hModule == IntPtr.Zero && CanLoadFromDisk)
                {
                    hModule = LoadModuleFromDisk(DLLName);
                    if (hModule == IntPtr.Zero)
                    {
                        throw new FileNotFoundException(DLLName + ", unable to find the specified file.");
                    }
                }
                else if (hModule == IntPtr.Zero)
                {
                    throw new DllNotFoundException(DLLName + ", Dll was not found.");
                }

                return GetExportAddress(hModule, FunctionName);
            }

            public static IntPtr GetExportAddress(IntPtr ModuleBase, string ExportName)
            {
                IntPtr FunctionPtr = IntPtr.Zero;
                try
                {
                    // Traverse the PE header in memory
                    Int32 PeHeader = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + 0x3C));
                    Int16 OptHeaderSize = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + PeHeader + 0x14));
                    Int64 OptHeader = ModuleBase.ToInt64() + PeHeader + 0x18;
                    Int16 Magic = Marshal.ReadInt16((IntPtr)OptHeader);
                    Int64 pExport = 0;
                    if (Magic == 0x010b)
                    {
                        pExport = OptHeader + 0x60;
                    }
                    else
                    {
                        pExport = OptHeader + 0x70;
                    }

                    // Read -> IMAGE_EXPORT_DIRECTORY
                    Int32 ExportRVA = Marshal.ReadInt32((IntPtr)pExport);
                    Int32 OrdinalBase = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x10));
                    Int32 NumberOfFunctions = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x14));
                    Int32 NumberOfNames = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x18));
                    Int32 FunctionsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x1C));
                    Int32 NamesRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x20));
                    Int32 OrdinalsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x24));

                    for (int i = 0; i < NumberOfNames; i++)
                    {
                        string FunctionName = Marshal.PtrToStringAnsi((IntPtr)(ModuleBase.ToInt64() + Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + NamesRVA + i * 4))));
                        if (FunctionName.Equals(ExportName, StringComparison.OrdinalIgnoreCase))
                        {
                            Int32 FunctionOrdinal = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + OrdinalsRVA + i * 2)) + OrdinalBase;
                            Int32 FunctionRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + FunctionsRVA + (4 * (FunctionOrdinal - OrdinalBase))));
                            FunctionPtr = (IntPtr)((Int64)ModuleBase + FunctionRVA);
                            break;
                        }
                    }
                }
                catch
                {
                    throw new InvalidOperationException("Failed to parse module exports.");
                }

                if (FunctionPtr == IntPtr.Zero)
                {
                    throw new MissingMethodException(ExportName + ", export not found.");
                }
                return FunctionPtr;
            }

        }

    }
}
