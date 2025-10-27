using UnityEngine;
using UnityEngine.UI;

public class PopupWin : PopupBase
{
    [SerializeField] private Button rewardButton;

    public override void Init()
    {
        base.Init();

        // setup for rewardButton
        rewardButton.onClick.RemoveAllListeners();
        rewardButton.onClick.AddListener(() =>
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

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }
}