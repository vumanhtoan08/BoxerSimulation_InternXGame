using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupInfoEnemy : PopupBase
{
    [Header("Enemy")]
    [SerializeField] private Text enemyName;
    [SerializeField] private Text enemyAttack;
    [SerializeField] private Text enemyHealth;
    [SerializeField] private Text enemyReward;
    [SerializeField] private Image image;

    [Header("Player")]
    [SerializeField] private Text playerAttack;
    [SerializeField] private Text playerHealth;
    [SerializeField] private Text playerStamina;

    [SerializeField] private Button fightButton;
    [SerializeField] private Button closePopup;

    public override void Init()
    {
        base.Init();

        // setup for fight button 
        fightButton.onClick.RemoveAllListeners();
        fightButton.onClick.AddListener(() =>
        {
            AnimateButton(fightButton.transform); // 🔹 thêm tween scale

            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f);
            Hide();

            Sequence seq = DOTween.Sequence();

            seq.Append(CanvasManager.Instance.DarkPanelActive())
                .AppendCallback(() =>
                {
                    GameManager.Instance.ChangeGameState(Game_State.Battle);
                    CanvasManager.Instance.MovePlayerToBattle();

                    PlayerController.Instance.Health.OnSettingHealthBeforeBattle();
                    CanvasManager.Instance.SetupUIBeforeBattle();
                    CanvasManager.Instance.OnUpdateUIPlayer();
                })
               .Append(CanvasManager.Instance.DarkPanelUnActive())
               .AppendCallback(() => SoundManager.Instance.PlaySound(SoundKey.FightStart, 0.7f, 0.7f))
               .AppendCallback(() => SoundManager.Instance.PlayBGM(SoundKey.BattleBGM, 0.4f));
        });

        // setup for close button
        closePopup.onClick.RemoveAllListeners();
        closePopup.onClick.AddListener(() =>
        {
            AnimateButton(closePopup.transform); // 🔹 thêm tween scale

            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f);
            Hide();
        });
    }

    public override void Show()
    {
        base.Show();
        SoundManager.Instance.PlaySound(SoundKey.Bubble, 0.3f, 0.5f);
        UpdateInfoEnemy();
        UpdateInfoPlayer();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void UpdateInfoEnemy()
    {
        EnemyRuntimeData enemyData = EnemyManager.Instance.EnemyController.RuntimeData;
        enemyName.text = enemyData.EnemyData.Name;
        enemyAttack.text = enemyData.EnemyData.Attack.ToString();
        enemyHealth.text = enemyData.EnemyData.Health.ToString();
        enemyReward.text = enemyData.EnemyData.Reward.ToString();

        // Thay Materials
        image.sprite = enemyData.EnemyData.Avatar;
        enemyData.EnemyMaterials.ChangMaterialForEnemy(enemyData.EnemyData.Material);
    }

    public void UpdateInfoPlayer()
    {
        PlayerRunTimeDatas playerData = PlayerController.Instance.Data;
        playerAttack.text = playerData.DataRuntime.Attack.ToString();
        playerHealth.text = playerData.DataRuntime.Health.ToString();
        playerStamina.text = playerData.DataRuntime.Stamina.ToString();
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
