using UnityEngine;
using UnityEngine.UI;

public class PopupNotEnoughEnergy : PopupBase
{
    [SerializeField] private Button acceptBtn;      // dùng để xác nhận 

    public override void Init()
    {
        base.Init();

        acceptBtn.onClick.RemoveAllListeners();
        acceptBtn.onClick.AddListener(() => 
        {
            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f); 
            Hide(); 
        });
    }

    public override void Show()
    {
        base.Show();
        SoundManager.Instance.PlaySound(SoundKey.Bubble, 0.3f, 0.5f);
    }
}
