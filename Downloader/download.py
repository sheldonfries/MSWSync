import os
import shutil
import subprocess
import zipfile
import config

# Configuration
ONEDRIVE_ZIP_PATH = os.path.join(config.onedrive_path, config.zip_file_name)
DEST_FOLDER = os.path.join(config.destination_path, config.zip_file_name)
WORKING_ZIP_PATH = os.path.join(config.download_path, config.zip_file_name)

def sync_onedrive():
    print("Syncing OneDrive...")
    subprocess.run(["onedrive", "--synchronize"], check=True)

def extract_zip():
    print(f"Extracting zip to {DEST_FOLDER}...")
    if DEST_FOLDER.exists():
        shutil.rmtree(DEST_FOLDER)
    DEST_FOLDER.mkdir(parents=True, exist_ok=True)
    with zipfile.ZipFile(WORKING_ZIP_PATH, "r") as zip_ref:
        zip_ref.extractall(DEST_FOLDER)

def delete_zip():
    if os.path.exists(WORKING_ZIP_PATH):
        print("Deleting zip file...")
        os.remove(WORKING_ZIP_PATH)


#sync_onedrive()
if ONEDRIVE_ZIP_PATH.exists():
    #shutil.copy2(ONEDRIVE_ZIP_PATH, WORKING_ZIP_PATH)
    subprocess.run(["rclone", "copy", ONEDRIVE_ZIP_PATH, DEST_FOLDER], check=True)
    extract_zip()
    print("Game files updated successfully.")
else:
    print(f"Zip file not found at {ONEDRIVE_ZIP_PATH}")
