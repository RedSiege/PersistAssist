using System;
using System.IO;

using static System.Console;
using static System.Runtime.InteropServices.Marshal;

using static PersistAssist.Utils.Structs.FileStructs;

namespace PersistAssist.Utils
{
    public class TimeStomp
    {
        // https://gist.github.com/Gr1mmie/d949b1804f733cd4335342bb56cea7d7#file-timestomp-cs

        //check if dir, file, or non-existent
        public static int PathType(string path)
        {
            try
            {
                FileAttributes FileAttrib = File.GetAttributes(path);

                if (FileAttrib == FileAttributes.Directory)
                {
                    // 0x2 - dir
                    return 0x2;
                }
                else
                {
                    // 0x1 - file
                    return 0x1;
                }
            }
            catch (FileNotFoundException)
            {
                // 0x0 - invalid path
                return 0x0;
            }

        }

        public static VALIDTIME VerifyInputTime(string timestamp)
        {
            VALIDTIME validTime = new VALIDTIME();
            DateTime dt = new DateTime();
            Random rnum = new Random();

            try
            {
                dt = DateTime.Parse(timestamp);
            }
            catch (FormatException)
            {
                validTime.isValid = false;
                return validTime;
            }

            // randomize if not provided
            if (dt.Millisecond == 0) { dt.AddMilliseconds(rnum.Next(0, 999)); }
            if (dt.Second == 0) { dt.AddSeconds(rnum.Next(0, 59)); }
            if (dt.Minute == 0) { dt.AddMinutes(rnum.Next(0, 59)); }
            if (dt.Hour == 0) { dt.AddHours(rnum.Next(8, 18)); }

            validTime.isValid = true;
            validTime.dTime = dt;

            return validTime;
        }

        public static ALLDATETIME ReturnObjTime(string path)
        {
            IntPtr hFile = IntPtr.Zero;
            ALLDATETIME objDT = new ALLDATETIME();
            int ObjType = PathType(path);

            IntPtr ptr = API.TSS.GetLibraryAddress("kernel32.dll", "CreateFileA");
            var CreateFile = GetDelegateForFunctionPointer(ptr, typeof(API.Delegates.CreateFile)) as API.Delegates.CreateFile;

            ptr = API.TSS.GetLibraryAddress("kernel32.dll", "GetLastError");
            var GetLastError = GetDelegateForFunctionPointer(ptr, typeof(API.Delegates.GetLastError)) as API.Delegates.GetLastError;

            ptr = API.TSS.GetLibraryAddress("kernel32.dll", "GetFileTime");
            var GetFileTime = GetDelegateForFunctionPointer(ptr, typeof(API.Delegates.GetFileTime)) as API.Delegates.GetFileTime;


            // if not valid path, set and return false ADT obj, if valid then open handle to obj 
            if (ObjType == 0x0)
            {
                objDT.isValid = false;
                return objDT;
            }
            else if (ObjType == 0x1)
            {
                // 0x3 - FILE_SHARE_READ + FILE_SHARE_WRITE
                hFile = CreateFile(path, EFileAccess.GenericRead, EFileShare.Read_Write, IntPtr.Zero, ECreationDisposition.OpenExisting, EFileAttributes.Normal, IntPtr.Zero);

            }
            else
            {
                hFile = CreateFile(path, EFileAccess.GenericRead, EFileShare.Read_Write, IntPtr.Zero, ECreationDisposition.OpenExisting, EFileAttributes.BackupSemantics, IntPtr.Zero);
            }

            if (hFile == (IntPtr)(-1))
            {
                WriteLine($"[-] Error: {GetLastError()}");
                objDT.isValid = false;
                return objDT;
            }
            else
            {
                FILETIME CreationTime = new FILETIME();
                FILETIME LastModifiedTime = new FILETIME();
                FILETIME LastAccessedTime = new FILETIME();

                bool success = GetFileTime(hFile, ref CreationTime, ref LastAccessedTime, ref LastModifiedTime);

                if (success)
                {
                    DateTime CreatedTime = DateTime.FromFileTimeUtc((((long)CreationTime.dwHighDateTime) << 32) | (long)CreationTime.dwLowDateTime);
                    DateTime ModifiedTime = DateTime.FromFileTimeUtc((((long)LastModifiedTime.dwHighDateTime) << 32) | (long)LastModifiedTime.dwLowDateTime);
                    DateTime AccessedTime = DateTime.FromFileTimeUtc(((long)LastAccessedTime.dwHighDateTime) << 32 | (long)LastAccessedTime.dwLowDateTime);


                    DateTime rCreatedTime = CreatedTime.ToUniversalTime().ToLocalTime();
                    DateTime rModifiedTime = ModifiedTime.ToUniversalTime().ToLocalTime();
                    DateTime rAccessedTime = AccessedTime.ToUniversalTime().ToLocalTime();

                    WriteLine($"[*] Path:\t{path}");

                    if (ObjType == 1) { WriteLine("[*] Path obj type:\tFile\n"); }
                    if (ObjType == 2) { WriteLine("[*] Path obj type:\tDirectory\n"); }

                    WriteLine("[*] Creation Time: " + rCreatedTime);
                    WriteLine("[*] Last Modified Time: " + rModifiedTime);
                    WriteLine("[*] Last Accessed Time: " + rAccessedTime);

                    objDT.isValid = true;
                    objDT.CreationTime = rCreatedTime;
                    objDT.LastWriteTime = rModifiedTime;
                    objDT.LastAccessTime = rAccessedTime;
                    return objDT;

                }
                else
                {
                    WriteLine($"[-] Failed to read obj filetime: {path}");
                    objDT.isValid = false;
                    return objDT;
                }
            }

        }

        public static bool SetTime(string path, DateTime date, bool SetCreationDate, bool SetAccessDate, bool SetModifiedDate, ALLDATETIME dup = new ALLDATETIME())
        {
            IntPtr hFile = IntPtr.Zero;
            int objType = PathType(path);

            IntPtr ptr = API.TSS.GetLibraryAddress("kernel32.dll", "CreateFileA");
            var CreateFile = GetDelegateForFunctionPointer(ptr, typeof(API.Delegates.CreateFile)) as API.Delegates.CreateFile;

            ptr = API.TSS.GetLibraryAddress("kernel32.dll", "GetLastError");
            var GetLastError = GetDelegateForFunctionPointer(ptr, typeof(API.Delegates.GetLastError)) as API.Delegates.GetLastError;

            ptr = API.TSS.GetLibraryAddress("kernel32.dll", "SetFileTime");
            var SetFileTime = GetDelegateForFunctionPointer(ptr, typeof(API.Delegates.SetFileTime)) as API.Delegates.SetFileTime;

            if (objType == 0) { WriteLine("[-] Invalid path"); return false; }
            if (objType == 1)
            {
                hFile = CreateFile(path, EFileAccess.FILE_WRITE_ATTRIBUTES, EFileShare.Read_Write, IntPtr.Zero, ECreationDisposition.OpenExisting,
                    EFileAttributes.Normal, IntPtr.Zero);
            }
            if (objType == 2)
            {
                hFile = CreateFile(path, EFileAccess.FILE_WRITE_ATTRIBUTES, EFileShare.Read_Write, IntPtr.Zero, ECreationDisposition.OpenExisting,
                    EFileAttributes.BackupSemantics, IntPtr.Zero);
            }

            if (hFile == IntPtr.Zero)
            {
                WriteLine($"[-] Error: {GetLastError()}");
                return false;
            }
            else
            {
                bool Success = false;

                if (dup.isValid)
                {
                    long CreateDate = dup.CreationTime.ToFileTime();
                    long ModifiedDate = dup.LastWriteTime.ToFileTime();
                    long AccessedDate = dup.LastAccessTime.ToFileTime();

                    Success = SetFileTime(hFile, ref CreateDate, ref ModifiedDate, ref AccessedDate);
                }
                else
                {
                    long dt = date.ToFileTime();

                    long nullTime = 0;

                    if (!SetCreationDate && !SetAccessDate && !SetModifiedDate) { Success = SetFileTime(hFile, ref dt, ref dt, ref dt); }
                    else
                    {
                        if (SetCreationDate && !SetAccessDate && !SetModifiedDate) { Success = SetFileTime(hFile, ref dt, ref nullTime, ref nullTime); }
                        if (SetCreationDate && SetAccessDate && !SetModifiedDate) { Success = SetFileTime(hFile, ref dt, ref dt, ref nullTime); }
                        if (SetCreationDate && !SetAccessDate && SetModifiedDate) { Success = SetFileTime(hFile, ref dt, ref nullTime, ref dt); }
                        if (!SetCreationDate && SetAccessDate && !SetModifiedDate) { Success = SetFileTime(hFile, ref nullTime, ref dt, ref nullTime); }
                        if (!SetCreationDate && SetAccessDate && SetModifiedDate) { Success = SetFileTime(hFile, ref nullTime, ref dt, ref dt); }
                        if (!SetCreationDate && !SetAccessDate && SetModifiedDate) { Success = SetFileTime(hFile, ref nullTime, ref nullTime, ref dt); }
                    }
                }
                if (Success) { return true; } else { return false; }
            }
        }

        public static void StompFromDupObjFileTime(string targetPath, string sourcePath)
        {
            WriteLine($"[*] MAC dates for {sourcePath}");
            ALLDATETIME sDT = ReturnObjTime(sourcePath);
            WriteLine();

            WriteLine($"[*] Old MAC dates for {targetPath}");
            ReturnObjTime(targetPath);
            WriteLine();

            if (!sDT.isValid) { WriteLine($"[-] Source path ({sourcePath}) ALLDATETIME value unable to be processed..."); return; }

            WriteLine($"[*] Attempting to TimeStomp MAC dates for {targetPath}\n");
            bool StompStatus = SetTime(targetPath, new DateTime(), false, false, false, sDT);

            if (StompStatus) {
                WriteLine($"[+] Successfully stomped {targetPath}\n");
                ReturnObjTime(targetPath);
            }
            else { WriteLine($"[-] Failed to stomp {targetPath}"); }
        }

        public static void StompCreationDate(string path, string newDate)
        {
            try
            {
                VALIDTIME validTime = VerifyInputTime(newDate);

                if (!validTime.isValid) { throw new ArgumentException("[-] Provided date object is invalid"); }

                WriteLine($"[*] Attempting to TimeStomp C(reate) date of object {path}\n");
                bool StompStatus = SetTime(path, validTime.dTime, true, false, false);

                if (StompStatus)
                {
                    WriteLine($"[+] Successfully stomped C(reate) date of object {path}\n");
                    ReturnObjTime(path);
                }
                else { throw new ArgumentException($"[-] Failed to stomp C(reate) date of object {path}"); }

            }
            catch (ArgumentException e) { WriteLine(e.Message); }
        }

        public static void StompLastAccessedDate(string path, string newDate)
        {
            try
            {
                VALIDTIME validTime = VerifyInputTime(newDate);

                if (!validTime.isValid) { throw new ArgumentException("[-] Provided date object is invalid"); }

                WriteLine($"[*] Attempting to TimeStomp A(ccessed) date of object {path}\n");
                bool StompStatus = SetTime(path, validTime.dTime, false, true, false);

                if (StompStatus)
                {
                    WriteLine($"[+] Successfully stomped A(ccessed) date of object {path}\n");
                    ReturnObjTime(path);
                }
                else { throw new ArgumentException($"[-] Failed to stomp A(ccessed) date of object {path}"); }
            }
            catch (ArgumentException e) { WriteLine(e.Message); }
        }

        public static void StompLastModifiedDate(string path, string newDate)
        {
            try
            {
                VALIDTIME validTime = VerifyInputTime(newDate);

                if (!validTime.isValid) { throw new ArgumentException("[-] Provided date object is invalid"); }

                WriteLine($"[*] Attempting to stomp M(odified) time of object {path}\n");
                bool StompStatus = SetTime(path, validTime.dTime, false, false, true);

                if (StompStatus)
                {
                    WriteLine($"[+] Successfully stomped M(odified) date of object {path}\n");
                    ReturnObjTime(path);
                }
                else { throw new ArgumentException($"[-] Failed to stomp M(odiifed) date of object {path}"); }
            }
            catch (ArgumentException e) { WriteLine(e.Message); }
        }

        public static void StompAll(string path, string newDate)
        {
            try
            {
                VALIDTIME validTime = VerifyInputTime(newDate);

                if (!validTime.isValid) { throw new ArgumentException("[-] Provided date object is invalid"); }

                WriteLine($"[*] Attempting to TimeStomp M(odified), A(ccessed), C(reated) dates of object {path}\n");
                bool StompStatus = SetTime(path, validTime.dTime, true, true, true);

                if (StompStatus)
                {
                    WriteLine($"[+] Successfully stomped M(odified), A(ccessed), C(reated) dates of object {path}\n");
                    ReturnObjTime(path);
                }
                else { throw new ArgumentException($"[-] Failed to stomp M(odified), A(ccessed), C(reated) dates of object {path}"); }
            }
            catch (ArgumentException e) { WriteLine(e.Message); }
        }

    }
}
