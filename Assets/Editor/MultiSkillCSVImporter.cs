using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class MultiSkillCSVImporter : EditorWindow
{
    private TextAsset csvFile;

    [MenuItem("Tools/CSV → Multi Skill Importer")]
    public static void ShowWindow()
    {
        GetWindow<MultiSkillCSVImporter>("Multi Skill CSV Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("💥 Import Skill Level Tables from CSV", EditorStyles.boldLabel);
        GUILayout.Space(10);

        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV File", csvFile, typeof(TextAsset), false);

        if (GUILayout.Button("Import CSV"))
        {
            if (csvFile == null)
            {
                EditorUtility.DisplayDialog("Error", "Please select a CSV file first!", "OK");
                return;
            }

            ImportSkills(csvFile);
        }
    }

    private void ImportSkills(TextAsset csv)
    {
        string[] lines = csv.text.Split('\n');
        if (lines.Length < 2)
        {
            Debug.LogError("CSV file seems empty or invalid.");
            return;
        }

        Dictionary<string, SkillLevelTable> skillTables = new();

        for (int i = 1; i < lines.Length; i++) // bỏ header
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] cols = line.Split(',');
            if (cols.Length < 3) continue;

            string id = cols[0].Trim();
            if (string.IsNullOrEmpty(id)) continue;

            if (!int.TryParse(cols[1], out int level)) continue;
            if (!float.TryParse(cols[2], out float cost)) continue;

            if (!skillTables.ContainsKey(id))
            {
                var table = ScriptableObject.CreateInstance<SkillLevelTable>();
                table.SkillID = id;
                table.SkillName = GetSkillNameByID(id);
                table.Levels = new List<SkillLevelTable.SkillLevelData>();
                skillTables.Add(id, table);
            }

            skillTables[id].Levels.Add(new SkillLevelTable.SkillLevelData
            {
                Level = level,
                Cost = cost
            });
        }

        string folder = "Assets/_Project/Data/Resources/Skills/";
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        foreach (var kvp in skillTables)
        {
            string id = kvp.Key;
            var table = kvp.Value;
            string assetPath = folder + $"Skill_{id}.asset";
            AssetDatabase.CreateAsset(table, assetPath);
            Debug.Log($"✅ Created {assetPath} ({table.Levels.Count} levels)");
        }

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
    }

    private string GetSkillNameByID(string id)
    {
        switch (id)
        {
            case "SK_01": return "Punch";
            case "SK_02": return "Kick";
            case "SK_03": return "Energy Burst";
            default: return "Unknown Skill";
        }
    }
}
