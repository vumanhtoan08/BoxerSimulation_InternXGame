using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupNotEnoughEnergy : PopupBase
{
    [SerializeField] private Button acceptBtn; // nút xác nhận

    public override void Init()
    {
        base.Init();

        acceptBtn.onClick.RemoveAllListeners();
        acceptBtn.onClick.AddListener(() =>
        {
            AnimateButton(acceptBtn.transform); // 🔹 hiệu ứng scale click

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
    /// 🔹 Hiệu ứng scale 1 → 1.1 → 1 trong 0.1s
    /// </summary>
    private void AnimateButton(Transform target)
    {
        target.DOKill(); // hủy tween cũ nếu đang chạy
        target.localScale = Vector3.one;
        target.DOScale(1.1f, 0.05f)
              .SetEase(Ease.OutQuad)
              .OnComplete(() => target.DOScale(1f, 0.05f).SetEase(Ease.InQuad));
    }
}
