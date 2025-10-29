using DG.Tweening;
using System;
using UnityEngine;

public class DayManager : Singleton<DayManager>
{
    [SerializeField] private DayData dataRuntime;

    [SerializeField] private int currentDay;
    [SerializeField] private PlayerRunTimeDatas data;

    public int CurrentDay => currentDay;

    public event Action OnNextDay;

    #region Unity Methods

    public void OnStart()
    {
        data = PlayerController.Instance.Data;
        dataRuntime.SetDataForDay();
        currentDay = dataRuntime.currentDay;
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
        dataRuntime.currentDay++;

        // 🔥 Đồng bộ với DataManager trước khi lưu
        DataManager.Instance.CurrentDayData.currentDay = dataRuntime.currentDay;

        DataManager.Instance.SaveData();

        currentDay = dataRuntime.currentDay;
        Debug.Log("🌅 Sang ngày mới: " + currentDay);

        // Dark Panel
        Sequence seq = DOTween.Sequence();
        seq.Append(CanvasManager.Instance.DarkPanelActive())
            .Append(CanvasManager.Instance.ShowDayText())
            .AppendCallback(() =>
            {
                PlayerController.Instance.transform.position = new Vector3(1, 0, 9);
                PlayerController.Instance.transform.rotation = Quaternion.Euler(0, 180, 0);
                PlayerController.Instance.CameraForInteract.transform.rotation = Quaternion.Euler(Vector3.zero);
            })
            .Append(CanvasManager.Instance.DarkPanelUnActive());

        OnNextDay?.Invoke();
    }
}
