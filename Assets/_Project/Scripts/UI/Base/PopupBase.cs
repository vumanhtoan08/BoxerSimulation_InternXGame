using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupBase : MonoBehaviour
{
    [Header("Popup Settings")]
    [SerializeField] private Type_Popup popupType;
    [SerializeField] private RectTransform popup;
    [SerializeField] private Image darkPanelImg;
    public Type_Popup PopupType => popupType;

    protected bool isActive;

    public virtual void Init()
    {

    }

    public virtual void Show()
    {
        // Setup 
        Color c = darkPanelImg.color;
        c.a = 0;
        darkPanelImg.color = c;
        popup.localScale = Vector3.zero;

        gameObject.SetActive(true);
        isActive = true;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(darkPanelImg.DOFade((float)150 / 255, 0.2f).SetEase(Ease.Linear))
            .Append(popup.DOScale(1, 0.2f).SetEase(Ease.OutBack));
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        isActive = false;
    }

    public bool IsActive() => isActive;
}
