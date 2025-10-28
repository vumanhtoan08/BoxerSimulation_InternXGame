using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    #region Unity Methods

    public void OnStart()
    {
        dataManager = DataManager.Instance;

        SetEventForButton();
        UpdateUIForShop();
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
        btnUpgradeBoxing.onClick.AddListener(() =>
        {
            OnButtonBoxingClick();
            UpdateUIForBoxing();
        });
        btnUpgradeRunning.onClick.AddListener(() =>
        {
            OnButtonRunningClick();
            UpdateUIForRunning();
        });
        btnUpgradeSquat.onClick.AddListener(() =>
        {
            OnButtonSquatClick();
            UpdateUIForSquat();
        });
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

        if (!WalletManager.Instance.OnCheckMoneyForBuy(interactables[0].Data.Cost))
        {
            Debug.Log($"Không đủ tiền để nâng cấp {interactables[0].Type.ToString()}");
            return; 
        }

        if (interactables.Count == 0)
        {
            Debug.LogWarning($"⚠️ Không tìm thấy interactable nào thuộc loại {type} trong scene!");
            return;
        }

        interactables[0].Data.UpgradeInteractable(type);
        WalletManager.Instance.OnMoneyChange(-interactables[0].Data.Cost);

        foreach (var item in interactables)
        {
            item.Data.SetDataForInteractable(type);
        }

        Debug.Log($"✅ Upgraded {type} interactables (Level: {interactables[0].Data.Level}) — Total objects refreshed: {interactables.Count}");
    }

    #endregion

    #region Shop UI Methods

    [SerializeField] private Image boxingFill;
    [SerializeField] private Image runningFill;
    [SerializeField] private Image squatFill;

    [SerializeField] private TextMeshProUGUI boxingUpgradeCost;
    [SerializeField] private TextMeshProUGUI runningUpgradeCost;
    [SerializeField] private TextMeshProUGUI squatUpgradeCost;

    private DataManager dataManager;

    private void UpdateUIForBoxing()
    {
        boxingFill.fillAmount = (float)dataManager.CurrentInteractableData.BoxingLevel / (dataManager.ListInteractableTable.InteractableTables[0].Levels.Count - 1);

        //
        if (dataManager.CurrentInteractableData.BoxingLevel != dataManager.ListInteractableTable.InteractableTables[0].Levels.Count - 1)
            boxingUpgradeCost.text = dataManager.ListInteractableTable.InteractableTables[0].Levels[dataManager.CurrentInteractableData.BoxingLevel].Cost.ToString();
        else
            boxingUpgradeCost.text = $"Max Level";
    }

    private void UpdateUIForRunning()
    {
        runningFill.fillAmount = (float)dataManager.CurrentInteractableData.RunningLevel / (dataManager.ListInteractableTable.InteractableTables[1].Levels.Count - 1);

        //
        if (dataManager.CurrentInteractableData.RunningLevel != dataManager.ListInteractableTable.InteractableTables[1].Levels.Count - 1)
            runningUpgradeCost.text = dataManager.ListInteractableTable.InteractableTables[1].Levels[dataManager.CurrentInteractableData.RunningLevel].Cost.ToString();
        else
            runningUpgradeCost.text = $"Max Level";
    }

    private void UpdateUIForSquat()
    {
        squatFill.fillAmount = (float)dataManager.CurrentInteractableData.SquatLevel / (dataManager.ListInteractableTable.InteractableTables[2].Levels.Count - 1);

        //
        if (dataManager.CurrentInteractableData.SquatLevel != dataManager.ListInteractableTable.InteractableTables[2].Levels.Count - 1)
            squatUpgradeCost.text = dataManager.ListInteractableTable.InteractableTables[2].Levels[dataManager.CurrentInteractableData.SquatLevel].Cost.ToString();
        else
            squatUpgradeCost.text = $"Max Level";
    }

    private void UpdateUIForShop()
    {
        boxingFill.fillAmount = (float)dataManager.CurrentInteractableData.BoxingLevel / (dataManager.ListInteractableTable.InteractableTables[0].Levels.Count - 1);
        runningFill.fillAmount = (float)dataManager.CurrentInteractableData.RunningLevel / (dataManager.ListInteractableTable.InteractableTables[1].Levels.Count - 1);
        squatFill.fillAmount = (float)dataManager.CurrentInteractableData.SquatLevel / (dataManager.ListInteractableTable.InteractableTables[2].Levels.Count - 1);

        //
        if (dataManager.CurrentInteractableData.BoxingLevel != dataManager.ListInteractableTable.InteractableTables[0].Levels.Count - 1)
            boxingUpgradeCost.text = dataManager.ListInteractableTable.InteractableTables[0].Levels[dataManager.CurrentInteractableData.BoxingLevel].Cost.ToString();
        else
            boxingUpgradeCost.text = $"Max Level";

        //
        if (dataManager.CurrentInteractableData.RunningLevel != dataManager.ListInteractableTable.InteractableTables[1].Levels.Count - 1)
            runningUpgradeCost.text = dataManager.ListInteractableTable.InteractableTables[1].Levels[dataManager.CurrentInteractableData.RunningLevel].Cost.ToString();
        else
            runningUpgradeCost.text = $"Max Level";

        //
        if (dataManager.CurrentInteractableData.SquatLevel != dataManager.ListInteractableTable.InteractableTables[2].Levels.Count - 1)
            squatUpgradeCost.text = dataManager.ListInteractableTable.InteractableTables[2].Levels[dataManager.CurrentInteractableData.SquatLevel].Cost.ToString();
        else
            squatUpgradeCost.text = $"Max Level";
    }

    #endregion
}
