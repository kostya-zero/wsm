using System;
using System.Runtime.InteropServices;

namespace wsm;

public static class ResourceManager
{
    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int SHLoadIndirectString(
        string pszSource,
        System.Text.StringBuilder pszOutBuf,
        int cchOutBuf,
        IntPtr pvReserved);

    public static string? ExpandResourceString(string resource)
    {
        var sb = new System.Text.StringBuilder(1024);
        var hr = SHLoadIndirectString(resource, sb, sb.Capacity, IntPtr.Zero);
        return hr == 0 ? sb.ToString() : null;
    }
}