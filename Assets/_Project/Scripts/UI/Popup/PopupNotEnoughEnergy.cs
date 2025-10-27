using UnityEngine;
using UnityEngine.UI;

public class PopupNotEnoughEnergy : PopupBase
{
    [SerializeField] private Button acceptBtn;      // dùng để xác nhận 

    public override void Init()
    {
        base.Init();

        acceptBtn.onClick.RemoveAllListeners();
        acceptBtn.onClick.AddListener(Hide);
    }
}
