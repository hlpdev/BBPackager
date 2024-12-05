using System.Runtime.InteropServices;
using System.Text;

namespace bbpackager;

internal class ExeResourceWriter {
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr BeginUpdateResource(string pFileName, bool bDeleteExistingResources);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool UpdateResource(
        IntPtr hUpdate,
        IntPtr lpType,
        IntPtr lpName,
        ushort wLanguage,
        byte[] lpData,
        uint cbData);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

    public static bool ChangeData(string filePath, string productName, string version, string copyright, string description, string company) {
        IntPtr handle = BeginUpdateResource(filePath, false);
        if (handle == IntPtr.Zero) {
            Logger.Warn("Failed to update executable resource information");
            return false;
        }

        try {
            byte[] versionInfo = BuildVersionInfo(productName, version, copyright, description, company);

            bool updateResult = UpdateResource(
                handle,
                new IntPtr(16),
                new IntPtr(1),
                0x0409,
                versionInfo,
                (uint)versionInfo.Length
            );

            if (!updateResult) {
                Logger.Warn("Failed to update executable resource information; make sure your version is formatted as " +
                            "X.X.X.X as any other format will prevent the version from being displayed in the binary's details.");
                EndUpdateResource(handle, false);
                return true;
            }

            if (!EndUpdateResource(handle, false)) {
                Logger.Warn("Failed to update executable resource information");
                return false;
            }

            Logger.Log("Successfully updated executable resource information");
            return true;
        } catch {
            Logger.Warn("Failed to update executable resource information");
            EndUpdateResource(handle, true);
            return false;
        }
    }

    private static byte[] BuildVersionInfo(string productName, string version, string copyright, string description, string company) {
        string[] versionParts = version.Split('.');
        
        string versionInfoTemplate = @"
VSVersionInfo
{
    FILEVERSION " + versionParts[0] + "," + versionParts[1] + "," + versionParts[2] + "," + versionParts[3] + @"
    PRODUCTVERSION " + versionParts[0] + "," + versionParts[1] + "," + versionParts[2] + "," + versionParts[3] + @"
    FILEFLAGSMASK 0x3fL
    FILEFLAGS 0x0L
    FILEOS 0x4L
    FILETYPE 0x1L
    FILESUBTYPE 0x0L
    {
        BLOCK ""StringFileInfo""
        {
            BLOCK ""040904b0""
            {
                VALUE """"CompanyName"""", "" + company + @""
                VALUE """"FileDescription"""", "" + description + @""
                VALUE """"FileVersion"""", "" + versionParts[0] + "","" + versionParts[1] + "","" + versionParts[2] + "","" + versionParts[3] + @""
                VALUE """"InternalName"""", "" + productName + @""
                VALUE """"LegalCopyright"""", "" + copyright + @""
                VALUE """"OriginalFileName"""", "" + productName + @""
                VALUE """"ProductName"""", "" + productName + @""
                VALUE """"ProductVersion"""", "" + versionParts[0] + "","" + versionParts[1] + "","" + versionParts[2] + "","" + versionParts[3] + @""
            }
        }
        BLOCK ""VarFileInfo""
        {
            VALUE ""Translation"", 0x0409 0x04B0
        }
    }
}";
        
        return Encoding.UTF8.GetBytes(versionInfoTemplate);
    }
}