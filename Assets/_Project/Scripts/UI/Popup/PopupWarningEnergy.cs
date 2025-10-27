using UnityEngine;
using UnityEngine.UI;

public class PopupWarningEnergy : PopupBase
{
    [SerializeField] private Button acceptBtn; 
    [SerializeField] private Button rejectBtn;      // chính là close button

    public override void Init()
    {
        base.Init();
        acceptBtn.onClick.RemoveAllListeners();
        acceptBtn.onClick.AddListener(() =>
        {
            DayManager.Instance.MoveToNextDay();
            Hide();
        });

        rejectBtn.onClick.RemoveAllListeners();
        rejectBtn.onClick.AddListener(Hide);
    }
}