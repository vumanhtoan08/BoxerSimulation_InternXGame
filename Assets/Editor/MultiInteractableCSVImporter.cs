using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class MultiInteractableCSVImporter : EditorWindow
{
    private TextAsset csvFile;

    [MenuItem("Tools/CSV → Multi Interactable Importer")]
    public static void ShowWindow()
    {
        GetWindow<MultiInteractableCSVImporter>("Multi Interactable CSV Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("📦 Import Interactable Tables from CSV", EditorStyles.boldLabel);
        GUILayout.Space(10);

        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV File", csvFile, typeof(TextAsset), false);

        if (GUILayout.Button("Import CSV"))
        {
            if (csvFile == null)
            {
                EditorUtility.DisplayDialog("Error", "Please select a CSV file first!", "OK");
                return;
            }

            ImportInteractables(csvFile);
        }
    }

    private void ImportInteractables(TextAsset csv)
    {
        string[] lines = csv.text.Split('\n');
        if (lines.Length < 2)
        {
            Debug.LogError("CSV file is empty or invalid.");
            return;
        }

        Dictionary<string, InteractableTable> interactableTables = new();

        // Đọc từng dòng (bỏ header nếu có)
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] cols = line.Split('\t', ',', ';'); // hỗ trợ nhiều dạng phân cách

            // CSV format: ID | Name | Level | Value | Cost
            if (cols.Length < 5) continue;

            string id = cols[0].Trim();
            string name = cols[1].Trim();
            if (!int.TryParse(cols[2], out int level)) level = 0;
            if (!int.TryParse(cols[3], out int value)) value = 0;
            if (!int.TryParse(cols[4], out int cost)) cost = 0;

            if (!interactableTables.ContainsKey(id))
            {
                var table = ScriptableObject.CreateInstance<InteractableTable>();
                table.ID = id;
                table.Name = name;
                table.Levels = new List<InteractableTable.InteractableLevelData>();
                interactableTables.Add(id, table);
            }

            interactableTables[id].Levels.Add(new InteractableTable.InteractableLevelData
            {
                Level = level,
                Value = value,
                Cost = cost
            });
        }

        // Lưu file asset
        string folder = "Assets/_Project/Data/Resources/Interactables/";
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        foreach (var kvp in interactableTables)
        {
            string id = kvp.Key;
            var table = kvp.Value;
            string assetPath = folder + $"Interactable_{id}.asset";

            // Nếu có asset cũ → cập nhật
            var existing = AssetDatabase.LoadAssetAtPath<InteractableTable>(assetPath);
            if (existing != null)
            {
                EditorUtility.CopySerialized(table, existing);
                Debug.Log($"♻️ Updated: {assetPath} ({table.Levels.Count} levels)");
            }
            else
            {
                AssetDatabase.CreateAsset(table, assetPath);
                Debug.Log($"✅ Created: {assetPath} ({table.Levels.Count} levels)");
            }

            DestroyImmediate(table);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();

        Debug.Log("🎉 Import Interactable Tables complete!");
    }
}
