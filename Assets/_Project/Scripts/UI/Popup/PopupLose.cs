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
            AnimateButton(closeBtn.transform); // 🔹 hiệu ứng scale

            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f);
            Hide();

            Sequence seq = DOTween.Sequence();

            seq.Append(CanvasManager.Instance.DarkPanelActive())
               .AppendCallback(() =>
               {
                   GameManager.Instance.ChangeGameState(Game_State.Training);
                   CanvasManager.Instance.MovePlayerToTraining();

                   SoundManager.Instance.PlayBGM(SoundKey.TrainingBGM, 0.7f);
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

    /// <summary>
    /// 🔹 Làm hiệu ứng scale 1 → 1.1 → 1 trong 0.1s
    /// </summary>
    private void AnimateButton(Transform target)
    {
        target.DOKill();
        target.localScale = Vector3.one;
        target.DOScale(1.1f, 0.05f)
              .SetEase(Ease.OutQuad)
              .OnComplete(() => target.DOScale(1f, 0.05f).SetEase(Ease.InQuad));
    }
}
