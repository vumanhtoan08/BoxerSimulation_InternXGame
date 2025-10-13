using UnityEditor;
using UnityEngine;
using System.IO;

public class DeletePersistentData : EditorWindow
{
    [MenuItem("Tools/Delete All Data")]
    public static void DeletePersistentFiles()
    {
        string path = Application.persistentDataPath;
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Directory.CreateDirectory(path); // Recreate the directory to avoid issues

            Debug.Log("All files in Persistent Data Path deleted!");
            EditorUtility.DisplayDialog("Success", "Persistent Data has been cleared!", "OK");
        }
        else
        {
            Debug.LogWarning("Persistent Data Path not found!");
        }
    }
    [MenuItem("Tools/Open Persistent Data Folder")]
    public static void OpenPersistentDataFolder()
    {
        string path = Application.persistentDataPath;
        EditorUtility.RevealInFinder(path); // Mac & Windows
        Debug.Log("Opened: " + path);
    }
}
