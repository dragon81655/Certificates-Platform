
public static class OSInfo
{
    private static volatile OSType _osType = OSType.None;
    public static bool IsWindows()
    {
        DetermineOS();
        return _osType == OSType.Windows;
    }
    public static bool IsLinux()
    {
        DetermineOS();

        return _osType == OSType.Linux;
    }
    public static bool IsMacOS()
    {
        DetermineOS();

        return _osType == OSType.MacOS;
    }

    public static OSType GetOSType()
    {
        DetermineOS();
        return _osType;
    }
    private static void DetermineOS()
    {
        if(_osType == OSType.None)
        {
            if(System.Runtime.InteropServices.RuntimeInformation
            .IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                _osType = OSType.Windows; return;
            }
            if (System.Runtime.InteropServices.RuntimeInformation
            .IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                _osType = OSType.Linux; return;
            }
            if (System.Runtime.InteropServices.RuntimeInformation
            .IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            {
                _osType = OSType.MacOS; return;
            }
            _osType = OSType.Unknown;
        }
    }
    public enum OSType
    {
        None,
        Windows,
        Linux,
        MacOS,
        Unknown
    }
}
        

