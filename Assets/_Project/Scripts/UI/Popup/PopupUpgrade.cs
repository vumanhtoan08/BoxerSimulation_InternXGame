using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupUpgrade : PopupBase
{
    [SerializeField] private Image currentLevelImg;
    [SerializeField] private Image previourLevelImg;

    [SerializeField] private Text currentLevelTxt; 
    [SerializeField] private Text previourLevelTxt;

    [SerializeField] private Text currentLevelInfoTxt; 
    [SerializeField] private Text previourLevelInfoTxt;

    [SerializeField] private Button closeBtn;

    [SerializeField] private Sprite atkImg; 
    [SerializeField] private Sprite healthImg; 
    [SerializeField] private Sprite staminaImg; 

    public override void Init()
    {
        base.Init();

        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(() =>
        {
            AnimateButton(closeBtn.transform);
            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f);
            Hide();
        });
    }

    public override void Show()
    {
        base.Show();
        OnUpdateInfoUpgrade(PlayerController.Instance.CameraForInteract.CurrentInteractable.Type);
    }

    private void OnUpdateInfoUpgrade(TYPE_TRAINING type)
    {
        switch (type)
        {
            case TYPE_TRAINING.BOXING:
                currentLevelImg.sprite = atkImg;
                previourLevelImg.sprite = atkImg;

                int atkLevel = DataManager.Instance.CurrentPlayerData.AttackLevel;
                int atkMax = DataManager.Instance.StatLevelTables.StatLevelTables[0].Levels.Count - 1;

                if (atkLevel > atkMax)
                {
                    currentLevelTxt.text = "Level Max";
                    previourLevelTxt.text = $"Level {atkMax}";
                    currentLevelInfoTxt.text = "Attack: Max";
                    previourLevelInfoTxt.text = $"Attack: {DataManager.Instance.StatLevelTables.StatLevelTables[0].Levels[atkMax].Value}";
                }
                else
                {
                    currentLevelTxt.text = $"Level {atkLevel}";
                    previourLevelTxt.text = $"Level {atkLevel - 1}";
                    currentLevelInfoTxt.text = $"Attack: {DataManager.Instance.StatLevelTables.StatLevelTables[0].Levels[atkLevel].Value}";
                    previourLevelInfoTxt.text = $"Attack: {DataManager.Instance.StatLevelTables.StatLevelTables[0].Levels[atkLevel - 1].Value}";
                }
                break;

            case TYPE_TRAINING.RUNING:
                currentLevelImg.sprite = staminaImg;
                previourLevelImg.sprite = staminaImg;

                int stmLevel = DataManager.Instance.CurrentPlayerData.StaminaLevel;
                int stmMax = DataManager.Instance.StatLevelTables.StatLevelTables[1].Levels.Count - 1;

                if (stmLevel > stmMax)
                {
                    currentLevelTxt.text = "Level Max";
                    previourLevelTxt.text = $"Level {stmMax}";
                    currentLevelInfoTxt.text = "Stamina: Max";
                    previourLevelInfoTxt.text = $"Stamina: {DataManager.Instance.StatLevelTables.StatLevelTables[1].Levels[stmMax].Value}";
                }
                else
                {
                    currentLevelTxt.text = $"Level {stmLevel}";
                    previourLevelTxt.text = $"Level {stmLevel - 1}";
                    currentLevelInfoTxt.text = $"Stamina: {DataManager.Instance.StatLevelTables.StatLevelTables[1].Levels[stmLevel].Value}";
                    previourLevelInfoTxt.text = $"Stamina: {DataManager.Instance.StatLevelTables.StatLevelTables[1].Levels[stmLevel - 1].Value}";
                }
                break;

            case TYPE_TRAINING.SQUAT:
                currentLevelImg.sprite = healthImg;
                previourLevelImg.sprite = healthImg;

                int hpLevel = DataManager.Instance.CurrentPlayerData.HealthLevel;
                int hpMax = DataManager.Instance.StatLevelTables.StatLevelTables[2].Levels.Count - 1;

                if (hpLevel > hpMax)
                {
                    currentLevelTxt.text = "Level Max";
                    previourLevelTxt.text = $"Level {hpMax}";
                    currentLevelInfoTxt.text = "Health: Max";
                    previourLevelInfoTxt.text = $"Health: {DataManager.Instance.StatLevelTables.StatLevelTables[2].Levels[hpMax].Value}";
                }
                else
                {
                    currentLevelTxt.text = $"Level {hpLevel}";
                    previourLevelTxt.text = $"Level {hpLevel - 1}";
                    currentLevelInfoTxt.text = $"Health: {DataManager.Instance.StatLevelTables.StatLevelTables[2].Levels[hpLevel].Value}";
                    previourLevelInfoTxt.text = $"Health: {DataManager.Instance.StatLevelTables.StatLevelTables[2].Levels[hpLevel - 1].Value}";
                }
                break;
        }
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