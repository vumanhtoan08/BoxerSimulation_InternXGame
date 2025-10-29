using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewStatLevelTable", menuName = "Database/Stat Level Table")]
public class StatLevelTable : ScriptableObject
{
    public string StatID;
    public string StatName;
    public List<LevelData> Levels = new();

    [Serializable]
    public class LevelData
    {
        public int Level;
        public float Value;
        public int ProgressToNext;
    }

    public float GetValueByLevel(int level)
    {
        var data = Levels.Find(l => l.Level == level);
        return data != null ? data.Value : 0;
    }
}
