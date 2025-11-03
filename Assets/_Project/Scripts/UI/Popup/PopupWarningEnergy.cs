using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupWarningEnergy : PopupBase
{
    [SerializeField] private Button acceptBtn;
    [SerializeField] private Button rejectBtn; // close button

    public override void Init()
    {
        base.Init();

        acceptBtn.onClick.RemoveAllListeners();
        acceptBtn.onClick.AddListener(() =>
        {
            AnimateButton(acceptBtn.transform); // 🔹 hiệu ứng scale
            DayManager.Instance.MoveToNextDay();
            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f);
            Hide();
        });

        rejectBtn.onClick.RemoveAllListeners();
        rejectBtn.onClick.AddListener(() =>
        {
            AnimateButton(rejectBtn.transform); // 🔹 hiệu ứng scale
            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f);
            Hide();
        });
    }

    public override void Show()
    {
        base.Show();
        SoundManager.Instance.PlaySound(SoundKey.Bubble, 0.3f, 0.5f);
    }

    /// <summary>
    /// 🔹 Làm hiệu ứng scale 1 → 1.1 → 1 trong 0.1s
    /// </summary>
    private void AnimateButton(Transform target)
    {
        target.DOKill(); // hủy tween cũ nếu có
        target.localScale = Vector3.one;
        target.DOScale(1.1f, 0.05f)
              .SetEase(Ease.OutQuad)
              .OnComplete(() => target.DOScale(1f, 0.05f).SetEase(Ease.InQuad));
    }
}
