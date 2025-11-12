using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupWin : PopupBase
{
    [SerializeField] private Text rewardValue;
    [SerializeField] private Button rewardButton;

    public override void Init()
    {
        base.Init();

        rewardButton.onClick.RemoveAllListeners();
        rewardButton.onClick.AddListener(() =>
        {
            AnimateButton(rewardButton.transform); // 🔹 hiệu ứng scale click

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
                   // Enemy level up
                   DataManager.Instance.CurrentEnemyData.Level++;
                   DataManager.Instance.SaveData();

                   // Refresh Enemy Data
                   EnemyManager.Instance.EnemyController.RuntimeData.EnemyData.SetDataForEnemy();
                   EnemyManager.Instance.EnemyController.Health.Init(EnemyManager.Instance.EnemyController);
                   EnemyManager.Instance.EnemyController.StateMachine.ChangeState(new EnemyIdleState(EnemyManager.Instance.EnemyController));

                   // Update UI & position
                   CanvasManager.Instance.OnUpdateUIEnemy();
                   CanvasManager.Instance.MoveEnemyToBattle();

                   if (!TutorialManager.Instance.Data.isPass)
                   {
                       Sequence seq = DOTween.Sequence();

                       seq.AppendCallback(() => TutorialManager.Instance.OnConversationActive(true)).AppendInterval(2f)
                       .AppendCallback(() => TutorialManager.Instance.OnConversationActive(false));
                   }
               });

            // Reward player
            WalletManager.Instance.OnMoneyChange(
                EnemyManager.Instance.EnemyController.RuntimeData.EnemyData.Reward
            );
        });
    }

    public override void Show()
    {
        base.Show();
        rewardValue.text = $"{EnemyManager.Instance.EnemyController.RuntimeData.EnemyData.Reward}";
        SoundManager.Instance.PlaySound(SoundKey.Win, 0.3f, 0.5f);
    }

    public override void Hide()
    {
        base.Hide();
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
