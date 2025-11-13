using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupShop : PopupBase
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button openButton; // nút Shop trong Canvas

    public override void Init()
    {
        base.Init();

        openButton.onClick.RemoveAllListeners();
        openButton.onClick.AddListener(() =>
        {
            AnimateButton(openButton.transform); // 🔹 hiệu ứng scale
            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f);
            Show();
        });

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            AnimateButton(closeButton.transform); // 🔹 hiệu ứng scale
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
        if (target == null) return;

        target.DOKill();
        target.localScale = Vector3.one;
        target.DOScale(1.1f, 0.05f)
              .SetEase(Ease.OutQuad)
              .OnComplete(() =>
                  target.DOScale(1f, 0.05f)
                        .SetEase(Ease.InQuad)
              );
    }
}
