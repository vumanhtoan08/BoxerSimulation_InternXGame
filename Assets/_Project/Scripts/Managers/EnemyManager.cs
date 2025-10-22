using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private EnemyController enemyController;
    public EnemyController EnemyController => enemyController;

    #region Unity Methods
    public void OnStart()
    {
        enemyController?.OnStart();
    }

    public void OnUpdate()
    {
        enemyController?.OnUpdate();
    }
    #endregion 
}
