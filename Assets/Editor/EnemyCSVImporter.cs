using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class EnemyCSVImporter : EditorWindow
{
    private TextAsset csvFile;

    [MenuItem("Tools/CSV → Enemy Importer")]
    public static void ShowWindow()
    {
        GetWindow<EnemyCSVImporter>("Enemy Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("👾 Import Enemy Data from CSV", EditorStyles.boldLabel);
        GUILayout.Space(10);

        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV File", csvFile, typeof(TextAsset), false);

        if (GUILayout.Button("Import CSV"))
        {
            if (csvFile == null)
            {
                EditorUtility.DisplayDialog("Error", "Please select a CSV file first!", "OK");
                return;
            }

            ImportEnemies(csvFile);
        }
    }

    private void ImportEnemies(TextAsset csv)
    {
        string[] lines = csv.text.Split('\n');
        if (lines.Length < 2)
        {
            Debug.LogError("CSV file is empty or invalid.");
            return;
        }

        string folder = "Assets/_Project/Data/Resources/Enemies/";
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        int importedCount = 0;

        // Duyệt từng dòng, bỏ dòng header
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] cols = line.Split(',');

            if (cols.Length < 5)
            {
                Debug.LogWarning($"⚠️ Invalid line {i + 1}: {line}");
                continue;
            }

            string id = cols[0].Trim();
            string name = cols[1].Trim();

            if (!int.TryParse(cols[2].Trim(), out int level))
            {
                Debug.LogWarning($"⚠️ Invalid level at line {i + 1}");
                continue;
            }

            if (!float.TryParse(cols[3].Trim(), out float attack))
            {
                Debug.LogWarning($"⚠️ Invalid attack at line {i + 1}");
                continue;
            }

            if (!float.TryParse(cols[4].Trim(), out float defense))
            {
                Debug.LogWarning($"⚠️ Invalid defense at line {i + 1}");
                continue;
            }

            // ✅ Tạo asset cho từng enemy
            var enemy = ScriptableObject.CreateInstance<EnemyStatData>();
            enemy.ID = id;
            enemy.Name = name;
            enemy.Level = level;
            enemy.Attack = attack;
            enemy.Defense = defense;

            string assetPath = folder + $"Enemy_{id}.asset";

            AssetDatabase.CreateAsset(enemy, assetPath);
            importedCount++;

            Debug.Log($"✅ Imported Enemy: {name} (Level {level}) → {assetPath}");
        }

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Debug.Log($"🎉 Import completed! Total enemies imported: {importedCount}");
    }
}
