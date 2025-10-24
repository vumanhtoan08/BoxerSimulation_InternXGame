using UnityEngine;

public class PopupBase : MonoBehaviour
{
    [Header("Popup Settings")]
    [SerializeField] private Type_Popup popupType;
    public Type_Popup PopupType => popupType;

    protected bool isActive;

    public virtual void Init()
    {

    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        isActive = true;
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        isActive = false;
    }

    public bool IsActive() => isActive;
}
