using System;
using UnityEngine;

public class DayManager : Singleton<DayManager>
{
    [SerializeField] private int currentDay = 1;
    [SerializeField] private PlayerRunTimeDatas data;

    public int CurrentDay => currentDay;

    public event Action OnNextDay;
    
    #region Unity Methods

    public void OnStart()
    {
        data = PlayerController.Instance.Data;
    }

    public void OnUpdate()
    {

    }

    #endregion

    public void CheckConditionToNextDay()
    {
        if (data.EnergyUse())
        {
            MoveToNextDay();
        }
        else
        {
            Debug.Log("con nang luong");
            PopupManager.Instance.ShowPopup(Type_Popup.WarningEnergy);
        }
    }

    public void MoveToNextDay()
    {
        currentDay += 1;
        Debug.Log("Sang ngay moi" + currentDay);
        OnNextDay?.Invoke();
    }
}
