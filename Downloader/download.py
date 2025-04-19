import os
import subprocess
import zipfile
import config

# Configuration
ONEDRIVE_ZIP_PATH = os.path.join(config.onedrive_path, config.zip_file_name)
WORKING_ZIP_PATH = os.path.join(config.download_path, config.zip_file_name)

def pull_from_onedrive():
    print("Pulling data from OneDrive...")
    subprocess.run(["rclone", "copy", ONEDRIVE_ZIP_PATH, config.download_path], check=True)

def extract_zip():
    print("Extracting zip folder...")
    with zipfile.ZipFile(WORKING_ZIP_PATH, "r") as zip_ref:
        zip_ref.extractall(config.destination_path)

def delete_zip():
    if os.path.exists(WORKING_ZIP_PATH):
        print("Deleting zip file...")
        os.remove(WORKING_ZIP_PATH)


try:
    pull_from_onedrive()
    extract_zip()
    delete_zip()
    print("Game files updated successfully.")
except Exception as e:
    print(f"Failed to download latest files from OneDrive - {str(e)}")
