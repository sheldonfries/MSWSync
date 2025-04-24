using System.IO.Compression;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

class Program
{
    static string DEFAULT_VERSION = "0";
    static string VERSION_FILE_NAME = "CurrentVersion.txt";

    static void Main()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string? gameFolder = config["LocalGameFolder"];
        string? executableName = config["ExecutableName"];
        string? oneDriveFolder = config["OneDriveSyncFolder"];
        string? archiveName = config["ArchiveName"];
        string versionFile = VERSION_FILE_NAME;

        string executablePath = Path.Combine(gameFolder!, executableName!);
        string archivePath = Path.Combine(oneDriveFolder!, archiveName!);
        string versionFilePath = Path.Combine(oneDriveFolder!, versionFile);

        string existingVersion = GetExistingVersion(versionFilePath);
        string currentVersion = GetExecutableVersion(executablePath, existingVersion);

        bool versionHasChanged = !currentVersion.Equals(existingVersion);
        if (versionHasChanged)
        {
            PrepareZipFolder(archivePath, gameFolder!);
            File.WriteAllText(versionFilePath, currentVersion);
        }
        else
        {
            Console.WriteLine("No change in version detected.");
        }

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

    static string GetExistingVersion(string? versionFilePath)
    {
        string version = DEFAULT_VERSION;
        if (File.Exists(versionFilePath))
        {
            version = File.ReadAllText(versionFilePath);
        }
        return version;
    }
    
    static string GetExecutableVersion(string? executablePath, string existingVersion)
    {
        var versionInfo = FileVersionInfo.GetVersionInfo(executablePath!);
        string fileVersion = string.Format("{0}.{1}.{2}.{3}", versionInfo.FileMajorPart,
                                                              versionInfo.FileMinorPart,
                                                              versionInfo.FileBuildPart,
                                                              versionInfo.FilePrivatePart);
        string currentVersion = fileVersion ?? DEFAULT_VERSION;
        return currentVersion;
    }

    static void PrepareZipFolder(string archivePath, string gameFolder)
    {
        if (File.Exists(archivePath))
        {
            Console.WriteLine("Deleting existing zip file...");
            File.Delete(archivePath);
            Console.WriteLine("Existing zip file deleted.");
        }
            
        Console.WriteLine("Writing zip file to OneDrive...");
        ZipFile.CreateFromDirectory(gameFolder, archivePath);
        Console.WriteLine("Wrote zip file to OneDrive.");
    }
}
