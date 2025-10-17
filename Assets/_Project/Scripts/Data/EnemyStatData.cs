using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStatData", menuName = "Database/Enemy Stat Data")]
public class EnemyStatData : ScriptableObject
{
    [Header("Enemy Basic Info")]
    public string ID;          // Mã định danh của Enemy (ví dụ: "EN001")
    public string Name;        // Tên hiển thị của Enemy (ví dụ: "Demon Warrior")
    public int Level;          // Cấp độ của Enemy

    [Header("Enemy Stats")]
    public float Attack;       // Chỉ số tấn công
    public float Defense;      // Chỉ số phòng thủ
}

[CreateAssetMenu(fileName = "NewEnemyDatabase", menuName = "Database/Enemy Database")]
public class EnemyStatDatabase : ScriptableObject
{
    public List<EnemyStatData> Enemies = new List<EnemyStatData>();
}