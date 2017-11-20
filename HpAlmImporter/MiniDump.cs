using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Inflectra.SpiraTest.AddOns.QualityCenterImporter
{
    public class MiniDump
    {
        public const int MiniDumpNormal = 0x00000000;
        public const int MiniDumpWithDataSegs = 0x00000001;
        public const int MiniDumpWithFullMemory = 0x00000002;
        public const int MiniDumpWithHandleData = 0x00000004;
        public const int MiniDumpFilterMemory = 0x00000008;
        public const int MiniDumpScanMemory = 0x00000010;
        public const int MiniDumpWithUnloadedModules = 0x00000020;
        public const int MiniDumpWithIndirectlyReferencedMemory = 0x00000040;
        public const int MiniDumpFilterModulePaths = 0x00000080;
        public const int MiniDumpWithProcessThreadData = 0x00000100;
        public const int MiniDumpWithPrivateReadWriteMemory = 0x00000200;
        public const int MiniDumpWithoutOptionalData = 0x00000400;
        public const int MiniDumpWithFullMemoryInfo = 0x00000800;
        public const int MiniDumpWithThreadInfo = 0x00001000;
        public const int MiniDumpWithCodeSegs = 0x00002000;

        [DllImport("dbghelp.dll")]
        public static extern bool MiniDumpWriteDump(IntPtr hProcess,
                                                    Int32 ProcessId,
                                                    IntPtr hFile,
                                                    int DumpType,
                                                    IntPtr ExceptionParam,
                                                    IntPtr UserStreamParam,
                                                    IntPtr CallackParam);

        public static void CreateMiniDump()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            using (FileStream fs = new FileStream(desktopPath + @"\\UnhandledDump.dmp", FileMode.Create))
            {
                using (System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess())
                {
                    MiniDumpWriteDump(process.Handle,
                                                     process.Id,
                                                     fs.SafeFileHandle.DangerousGetHandle(),
                                                     MiniDump.MiniDumpNormal,
                                                     IntPtr.Zero,
                                                     IntPtr.Zero,
                                                     IntPtr.Zero);
                }
            }
        }

    }
}
