# MSW Sync

MapleStory Worlds cannot be installed on a Linux machine, but the game does run perfectly fine if the install files are moved onto the Linux machine from a Windows machine. This tool will allow you 
to (somewhat) automate the process of copying the install files from your Windows machine to your Linux machine by using OneDrive.

This tool has two parts: the uploader, and the downloader. The uploader sends a zip file of the folder contents to OneDrive, and the downloader pulls the contents to another local machine. The uploader will
only upload a new copy of the zip file if it detects a change in the game files.

## Requirements
- Windows PC with MapleStory Worlds installed to upload the installed files
- Linux machine (e.g. Steam Deck) with rclone installed to download the files

## Steps

**Uploader**
1. If not already installed, download and install [MapleStory Worlds](https://maplestoryworlds.nexon.com/en/play) on your Windows machine.
1. Download the **Uploader** portion of the MSWSync tool on your Windows machine.
1. Open the `appsettings.json` file in a text editor and edit the configurations to point to the appropriate locations on your machine. See [Windows Configurations](#windows-configurations) for more detail.
1. Run the **Uploader** to push a zipped copy of your MSW folder to your OneDrive folder.

**Downloader**
1. Install [rclone](https://rclone.org/downloads/) on your Linux machine.
1. Download the **Downloader** portion of the MSWSync tool on your Linux machine.
1. Open the `config.py` in a text editor and edit the configurations to point to the appropriate locations on your machine. See [Linux Configurations](#linux-configurations) for more detail.
1. Run the **Downloader** to pull the zip file from OneDrive, unzip it, and move the contents to your MSW folder.
1. (Optional) If running on a Steam Deck, add the unzipped location to Steam as a Non-Steam Game to enable running from Gaming Mode.

## Windows Configurations
- LocalGameFolder: The folder containing your MSW installation.
- OneDriveSyncFolder: The OneDrive folder that will hold the zipped install files.
- ArchiveName: The name of the zip archive that the Uploader will create. This should match the zip_file_name configuration in the **Downloader**.
- ExecutableName: The name of the executable file in your MSW installation folder.

## Linux Configurations
- onedrive_path: The OneDrive folder that holds the zipped install files.
- download_path: The path to your Downloads folder.
- destination_path: The folder that will store your game files.
- zip_file_name: The name of the zip archive in the onedrive_path. This should match the ArchiveName configuration in the **Uploader**.