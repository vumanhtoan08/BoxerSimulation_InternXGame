using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewSkillLevelTable", menuName = "Database/Skill Level Table")]
public class SkillLevelTable : ScriptableObject
{
    public string SkillID;
    public string SkillName;
    public List<SkillLevelData> Levels = new();

    [Serializable]
    public class SkillLevelData
    {
        public int Level;
        public float Cost;
    }

    public float GetBaseCostByLevel(int level)
    {
        var data = Levels.Find(l => l.Level == level);
        return data != null ? data.Cost : 0;
    }
}
