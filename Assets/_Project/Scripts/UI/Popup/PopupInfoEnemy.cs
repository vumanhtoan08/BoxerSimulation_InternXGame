
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupInfoEnemy : PopupBase
{
    [Header("Enemy")]
    [SerializeField] private TextMeshProUGUI enemyName; 
    [SerializeField] private TextMeshProUGUI enemyAttack; 
    [SerializeField] private TextMeshProUGUI enemyHealth; 
    [SerializeField] private TextMeshProUGUI enemyReward; 
    [SerializeField] private Image image;

    [Header("Player")]
    [SerializeField] private TextMeshProUGUI playerAttack;
    [SerializeField] private TextMeshProUGUI playerHealth;
    [SerializeField] private TextMeshProUGUI playerStamina;

    [SerializeField] private Button fightButton;
    [SerializeField] private Button closePopup; 

    public override void Init()
    {   
        base.Init();

        // setup for fight button 
        fightButton.onClick.RemoveAllListeners();
        fightButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f);
            Hide();

            Sequence seq = DOTween.Sequence();

            seq.Append(CanvasManager.Instance.DarkPanelActive())
                .AppendCallback(() =>
                {
                    GameManager.Instance.ChangeGameState(Game_State.Battle);
                    CanvasManager.Instance.MovePlayerToBattle();
                })
               .Append(CanvasManager.Instance.DarkPanelUnActive())
               .AppendCallback(() => SoundManager.Instance.PlaySound(SoundKey.FightStart, 0.7f, 0.7f))
               .AppendCallback(() => SoundManager.Instance.PlayBGM(SoundKey.BattleBGM, 0.4f));

            PlayerController.Instance.Health.OnSettingHealthBeforeBattle();
            CanvasManager.Instance.OnUpdateUIPlayer();
        });

        // setup for close button
        closePopup.onClick.RemoveAllListeners();
        closePopup.onClick.AddListener(() => { SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f); Hide(); });
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
    }

    public void UpdateInfoPlayer()
    {
        PlayerRunTimeDatas playerData = PlayerController.Instance.Data;
        playerAttack.text = playerData.DataRuntime.Attack.ToString();
        playerHealth.text = playerData.DataRuntime.Health.ToString();
        playerStamina.text = playerData.DataRuntime.Stamina.ToString();
    }
}