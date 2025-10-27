using UnityEngine;
using UnityEngine.UI;

public class PopupShop : PopupBase
{
    [SerializeField] private Image fillAmountPunchingBag; 
    [SerializeField] private Image fillAmountRunningMachine; 
    [SerializeField] private Image fillAmountSquat;
    [SerializeField] private Button closeButton;

    public override void Init()
    {
        base.Init();
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(Hide);
    }

    public void UpdateDataForShop()
    {
        fillAmountPunchingBag.fillAmount = DataManager.Instance.CurrentInteractableData.BoxingLevel / 5;
        fillAmountRunningMachine.fillAmount = DataManager.Instance.CurrentInteractableData.RunningLevel / 5;
        fillAmountSquat.fillAmount = DataManager.Instance.CurrentInteractableData.SquatLevel / 5;
    }
}