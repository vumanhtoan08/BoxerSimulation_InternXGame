using UnityEngine;
using UnityEngine.UI;

public class PopupLose : PopupBase
{
    [SerializeField] private Button closeBtn;

    public override void Init()
    {
        base.Init();

        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(() =>
        {
            CanvasManager.Instance.MovePlayerToTraining();
            Hide();
            GameManager.Instance.ChangeGameState(Game_State.Training);
            EnemyManager.Instance.EnemyController.Health.Init(EnemyManager.Instance.EnemyController);
            EnemyManager.Instance.EnemyController.StateMachine.ChangeState(new EnemyIdleState(EnemyManager.Instance.EnemyController));
            CanvasManager.Instance.MoveEnemyToBattle();
            CanvasManager.Instance.OnUpdateUIEnemy();
        });
    }
}