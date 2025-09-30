using System.IO.Compression;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Security.Cryptography;

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

        string existingChecksum = GetExistingChecksum(versionFilePath);
        string currentChecksum = GetExecutableChecksum(executablePath);

        bool versionHasChanged = !currentChecksum.Equals(existingChecksum);
        if (versionHasChanged)
        {
            PrepareZipFolder(archivePath, gameFolder!);
            File.WriteAllText(versionFilePath, currentChecksum);
        }
        else
        {
            Console.WriteLine("No change in version detected.");
        }

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

    static string GetExistingChecksum(string? versionFilePath)
    {
        string version = DEFAULT_VERSION;
        if (File.Exists(versionFilePath))
        {
            version = File.ReadAllText(versionFilePath);
        }
        return version;
    }

    static string GetExecutableChecksum(string executablePath)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(executablePath))
            {
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
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
