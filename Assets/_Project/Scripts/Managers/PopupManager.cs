using System.Collections.Generic;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] private List<PopupBase> popupList = new List<PopupBase>();

    #region Unity Methods
    public void OnStart()
    {
        foreach (var popup in popupList)
        {
            popup.Init();
        }

        foreach (var popup in popupList)
        {
            popup.gameObject.SetActive(false);
        }
    }

    public void OnUpdate()
    {

    }

    #endregion

    public void ShowPopup(Type_Popup type)
    {
        var popup = popupList.Find(p => p.PopupType == type);
        popup.Show();
    }

    public void HidePopup()
    {

    }
}

public enum Type_Popup
{
    None, 
    InfoEnemy, 
    Win, 
    Lose, 
    Shop, 
    WarningEnergy,
    NotEnoughEnergy,
    Setting,
    Upgrade
}