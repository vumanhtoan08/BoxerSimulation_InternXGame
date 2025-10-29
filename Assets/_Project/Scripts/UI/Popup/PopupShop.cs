using UnityEngine;
using UnityEngine.UI;

public class PopupShop : PopupBase
{
    [SerializeField] private Image fillAmountPunchingBag;
    [SerializeField] private Image fillAmountRunningMachine;
    [SerializeField] private Image fillAmountSquat;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button openButton; // chính là nút Shop trong Canvas

    public override void Init()
    {
        base.Init();

        openButton.onClick.RemoveAllListeners();
        openButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f);
            Show();
        });

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f);
            Hide();
        });
    }

    public void UpdateDataForShop()
    {
        fillAmountPunchingBag.fillAmount = DataManager.Instance.CurrentInteractableData.BoxingLevel / 5;
        fillAmountRunningMachine.fillAmount = DataManager.Instance.CurrentInteractableData.RunningLevel / 5;
        fillAmountSquat.fillAmount = DataManager.Instance.CurrentInteractableData.SquatLevel / 5;
    }

    public override void Show()
    {
        base.Show();
        SoundManager.Instance.PlaySound(SoundKey.Bubble, 0.3f, 0.5f);
    }
}