using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatLevelTable", menuName = "Database/ListStat")]
public class ListStatLevelTable : ScriptableObject
{
    public List<StatLevelTable> StatLevelTables = new List<StatLevelTable>();
}