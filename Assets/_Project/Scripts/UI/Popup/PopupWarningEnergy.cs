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
            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f);
            Hide();
        });

        rejectBtn.onClick.RemoveAllListeners();
        rejectBtn.onClick.AddListener(() => { SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f); Hide(); });
    }

    public override void Show()
    {
        base.Show();
        SoundManager.Instance.PlaySound(SoundKey.Bubble, 0.3f, 0.5f);
    }
}