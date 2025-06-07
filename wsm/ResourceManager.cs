using System;
using System.Runtime.InteropServices;

namespace wsm;

public static class ResourceManager
{
    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int SHLoadIndirectString(
        string pszSource,
        System.Text.StringBuilder pszOutBuf,
        int cchOutBuf,
        IntPtr pvReserved);

    public static string? ExpandResourceString(string resource)
    {
        var sb = new System.Text.StringBuilder(1024);
        int hr = SHLoadIndirectString(resource, sb, sb.Capacity, IntPtr.Zero);
        if (hr == 0)
            return sb.ToString();
        return null;
    }
}