using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    #region Unity Methods

    public void OnStart()
    {
        //SetEventForButton();
    }

    public void OnUpdate()
    {

    }

    #endregion

    #region Shop Behaviour Methods

    [SerializeField] private Button btnUpgradeBoxing; 
    [SerializeField] private Button btnUpgradeRunning; 
    [SerializeField] private Button btnUpgradeSquat;
    
    private void SetEventForButton()
    {
        btnUpgradeBoxing.onClick.AddListener(OnButtonBoxingClick);
        btnUpgradeRunning.onClick.AddListener(OnButtonRunningClick);
        btnUpgradeSquat.onClick.AddListener(OnButtonSquatClick);
    }

    public void OnButtonBoxingClick()
    {
        OnUpgradeInteractable(TYPE_TRAINING.BOXING);
    }

    public void OnButtonRunningClick()
    {
        OnUpgradeInteractable(TYPE_TRAINING.RUNING);
    }
    public void OnButtonSquatClick()
    {
        OnUpgradeInteractable(TYPE_TRAINING.SQUAT);
    }

    private void OnUpgradeInteractable(TYPE_TRAINING type)
    {
        List<InteractableBase> interactables = InteractableManager.Instance.InteractableLists
            .FindAll(i => i.Type == type);

        if (interactables.Count == 0)
        {
            Debug.LogWarning($"⚠️ Không tìm thấy interactable nào thuộc loại {type} trong scene!");
            return;
        }

        interactables[0].Data.UpgradeInteractable(type);

        foreach (var item in interactables)
        {
            item.Data.SetDataForInteractable(type);
        }

        Debug.Log($"✅ Upgraded {type} interactables (Level: {interactables[0].Data.Level}) — Total objects refreshed: {interactables.Count}");  
    }

    #endregion
}
