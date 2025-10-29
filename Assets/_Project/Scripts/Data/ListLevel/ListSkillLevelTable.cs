using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillLevelTable", menuName = "Database/ListSkill")]
public class ListSkillLevelTable : ScriptableObject
{
    public List<SkillLevelTable> StatLevelTables = new List<SkillLevelTable>();
}