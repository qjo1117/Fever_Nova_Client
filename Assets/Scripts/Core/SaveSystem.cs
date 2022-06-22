using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem 
{
	public static readonly string SAVE_FOLDER = Application.dataPath + "/Data/Saves/";

    public static void Init()
	{ 
		if(!Directory.Exists(SAVE_FOLDER)) {
			Directory.CreateDirectory(SAVE_FOLDER);
		}


	}

	public static void Save(string saveString)
	{
		int saveNumber = 1;
		//while(File.Exists("save_" + saveNumber + ".txt")) {
		//	saveNumber += 1;
		//}

		File.WriteAllText(SAVE_FOLDER + "save" + saveNumber + ".txt", saveString);


	}


	public static string Load()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
		FileInfo[] saveFiles = directoryInfo.GetFiles("*.txt");
		FileInfo mostRecentFile = null;

		foreach(FileInfo fileInfo in saveFiles) {
			if (mostRecentFile == null) {
				mostRecentFile = fileInfo;
			}
			else {
				if(fileInfo.LastWriteTime > mostRecentFile.LastWriteTime) {
					mostRecentFile = fileInfo;
				}
			}
		}

		if (mostRecentFile != null) {
			string saveString = File.ReadAllText(mostRecentFile.FullName);
			return saveString;
		}

		if(File.Exists(SAVE_FOLDER + "save.txt")) {
			string saveString = File.ReadAllText(SAVE_FOLDER + "save.txt");
			return saveString;
		}
		else {
			return null;
		}
	}



}
