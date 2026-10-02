using System.Runtime.InteropServices;

namespace PooSee.Services;

public sealed class SystemResourceService : ISystemResourceService
{
    public int GetLogicalProcessorCount() => Environment.ProcessorCount;

    public ulong GetTotalPhysicalMemoryBytes()
    {
        try
        {
            var status = new MEMORYSTATUSEX();
            status.dwLength = (uint)Marshal.SizeOf<MEMORYSTATUSEX>();
            if (GlobalMemoryStatusEx(ref status))
                return status.ullTotalPhys;
        }
        catch { }
        return 4UL * 1024 * 1024 * 1024;
    }

    public int GetRecommendedSessionCount()
    {
        var cores = GetLogicalProcessorCount();
        var ramGb = GetTotalPhysicalMemoryBytes() / (1024.0 * 1024.0 * 1024.0);
        var byRam = (int)Math.Floor(ramGb / 1.5);
        var rec = Math.Min(cores, Math.Max(1, byRam));
        return Math.Clamp(rec, 1, 10);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);
}