using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyDatabase", menuName = "Database/Enemy Database")]
public class EnemyStatDatabase : ScriptableObject
{
    public List<EnemyStatData> Enemies = new List<EnemyStatData>();
}