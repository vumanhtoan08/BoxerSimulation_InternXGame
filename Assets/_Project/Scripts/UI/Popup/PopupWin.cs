using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupWin : PopupBase
{
    [SerializeField] private TextMeshProUGUI rewardValue; 
    [SerializeField] private Button rewardButton;

    public override void Init()
    {
        base.Init();

        // setup for rewardButton
        rewardButton.onClick.RemoveAllListeners();
        rewardButton.onClick.AddListener(() =>
        {
            Hide();

            Sequence seq = DOTween.Sequence();

            seq.Append(CanvasManager.Instance.DarkPanelActive())
               .AppendCallback(() =>
               {
                   GameManager.Instance.ChangeGameState(Game_State.Training);
                   CanvasManager.Instance.MovePlayerToTraining();
               })
               .Append(CanvasManager.Instance.DarkPanelUnActive())
               .AppendCallback(() => 
               {
                   // Enemy
                   DataManager.Instance.CurrentEnemyData.Level++;
                   DataManager.Instance.SaveData();
                   // SetData mới cho Enemy
                   EnemyManager.Instance.EnemyController.RuntimeData.EnemyData.SetDataForEnemy();

                   EnemyManager.Instance.EnemyController.Health.Init(EnemyManager.Instance.EnemyController);
                   EnemyManager.Instance.EnemyController.StateMachine.ChangeState(new EnemyIdleState(EnemyManager.Instance.EnemyController));

                   CanvasManager.Instance.OnUpdateUIEnemy();
                   CanvasManager.Instance.MoveEnemyToBattle();
               });

            // Wallet
            WalletManager.Instance.OnMoneyChange(EnemyManager.Instance.EnemyController.RuntimeData.EnemyData.Reward);
        });
    }

    public override void Show()
    {
        base.Show();
        rewardValue.text = $"{EnemyManager.Instance.EnemyController.RuntimeData.EnemyData.Reward}";
    }

    public override void Hide()
    {
        base.Hide();
    }
}