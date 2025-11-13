using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class MultiStatCSVImporter : EditorWindow
{
    private TextAsset csvFile;

    [MenuItem("Tools/CSV → Multi Stat Importer")]
    public static void ShowWindow()
    {
        GetWindow<MultiStatCSVImporter>("Multi Stat Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("📊 Import Stat Level Tables from CSV", EditorStyles.boldLabel);
        GUILayout.Space(10);

        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV File", csvFile, typeof(TextAsset), false);

        if (GUILayout.Button("Import CSV"))
        {
            if (csvFile == null)
            {
                EditorUtility.DisplayDialog("Error", "Please select a CSV file first!", "OK");
                return;
            }

            ImportStats(csvFile);
        }
    }

    private void ImportStats(TextAsset csv)
    {
        string[] lines = csv.text.Split('\n');
        if (lines.Length < 2)
        {
            Debug.LogError("CSV file is empty or invalid.");
            return;
        }

        // Dictionary chứa các bảng Stat riêng biệt
        Dictionary<string, StatLevelTable> statTables = new();

        for (int i = 1; i < lines.Length; i++) // bỏ dòng header
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] cols = line.Split(',');
            if (cols.Length < 4) continue;

            string id = cols[0].Trim();
            if (string.IsNullOrEmpty(id)) continue;

            if (!int.TryParse(cols[1], out int level)) continue;
            if (!float.TryParse(cols[2], out float value)) continue;

            int progress = 0;
            string progressStr = cols[3].Trim().ToLower();
            if (progressStr == "max" || progressStr == "none") progress = 0;
            else int.TryParse(progressStr, out progress);

            // Nếu chưa có bảng cho ID này -> tạo mới
            if (!statTables.ContainsKey(id))
            {
                var table = ScriptableObject.CreateInstance<StatLevelTable>();
                table.StatID = id;
                table.StatName = GetStatNameByID(id);
                table.Levels = new List<StatLevelTable.LevelData>();
                statTables.Add(id, table);
            }

            // Thêm level vào bảng
            statTables[id].Levels.Add(new StatLevelTable.LevelData
            {
                Level = level,
                Value = value,
                ProgressToNext = progress
            });
        }

        string folder = "Assets/_Project/Data/Resources/Stats/";
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        // Lưu từng bảng ra file .asset
        foreach (var kvp in statTables)
        {
            string id = kvp.Key;
            var stat = kvp.Value;

            string assetPath = folder + $"Stat_{id}.asset";
            AssetDatabase.CreateAsset(stat, assetPath);
            Debug.Log($"✅ Created {assetPath} ({stat.Levels.Count} levels)");
        }

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
    }

    private string GetStatNameByID(string id)
    {
        switch (id)
        {
            case "S_01": return "Attack";
            case "S_02": return "Health";
            case "S_03": return "Stamina";
            default: return "Unknown";
        }
    }
}
