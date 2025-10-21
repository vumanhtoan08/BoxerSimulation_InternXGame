using UnityEngine;

public class EnemyRuntimeData : MonoBehaviour
{
    private EnemyData enemyData = new();
    public EnemyData EnemyData => enemyData;

    public void OnStart()
    {
        enemyData.SetDataForEnemy();
    }
}
