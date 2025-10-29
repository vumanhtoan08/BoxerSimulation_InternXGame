using DG.Tweening;
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
            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f);
            Hide();

            Sequence seq = DOTween.Sequence();

            seq.Append(CanvasManager.Instance.DarkPanelActive())
               .AppendCallback(() =>
               {
                   GameManager.Instance.ChangeGameState(Game_State.Training);
                   CanvasManager.Instance.MovePlayerToTraining();

                   SoundManager.Instance.StopBGM();
               })
               .Append(CanvasManager.Instance.DarkPanelUnActive())
               .AppendCallback(() =>
               {
                   EnemyManager.Instance.EnemyController.Health.Init(EnemyManager.Instance.EnemyController);
                   EnemyManager.Instance.EnemyController.StateMachine.ChangeState(new EnemyIdleState(EnemyManager.Instance.EnemyController));

                   CanvasManager.Instance.OnUpdateUIEnemy();
                   CanvasManager.Instance.MoveEnemyToBattle();
               });
        });
    }

    public override void Show()
    {
        base.Show();
        SoundManager.Instance.PlaySound(SoundKey.Lose, 0.3f, 0.5f);
    }
}