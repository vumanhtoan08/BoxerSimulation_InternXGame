
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
        if (DataManager.Instance.CurrentInteractableData.BoxingLevel == DataManager.Instance.ListInteractableTable.InteractableTables[0].Levels.Count - 1)
        {
            btnUpgradeBoxing.interactable = false;
            return;
        }
        else
        {
            btnUpgradeBoxing.interactable = true;
        }

        OnUpgradeInteractable(TYPE_TRAINING.BOXING);
    }

    public void OnButtonRunningClick()
    {
        if (DataManager.Instance.CurrentInteractableData.RunningLevel == DataManager.Instance.ListInteractableTable.InteractableTables[1].Levels.Count - 1)
        {
            btnUpgradeRunning.interactable = false;
            return;
        }
        else
            btnUpgradeRunning.interactable= true;

        OnUpgradeInteractable(TYPE_TRAINING.RUNING);
    }
    public void OnButtonSquatClick()
    {
        if (DataManager.Instance.CurrentInteractableData.SquatLevel == DataManager.Instance.ListInteractableTable.InteractableTables[2].Levels.Count - 1)
        {
            btnUpgradeSquat.interactable = false;
            return;
        }
        else
            btnUpgradeSquat.interactable = true;

        OnUpgradeInteractable(TYPE_TRAINING.SQUAT);
    }

    private void OnUpgradeInteractable(TYPE_TRAINING type)
    {
        List<InteractableBase> interactables = InteractableManager.Instance.InteractableLists
            .FindAll(i => i.Type == type);

        if (!WalletManager.Instance.OnCheckMoneyForBuy(interactables[0].Data.Cost))
        {
            Debug.Log($"Không đủ tiền để nâng cấp {interactables[0].Type.ToString()} == Số tiền {interactables[0].Data.Cost}");
            return;
        }

        if (interactables.Count == 0)
        {
            Debug.LogWarning($"⚠️ Không tìm thấy interactable nào thuộc loại {type} trong scene!");
            return;
        }

        SoundManager.Instance.PlaySound(SoundKey.Cash, 0.7f, 0.7f);
        WalletManager.Instance.OnMoneyChange(-interactables[0].Data.Cost);
        interactables[0].Data.UpgradeInteractable(type);

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

    [SerializeField] private Text boxingUpgradeCost;
    [SerializeField] private Text runningUpgradeCost;
    [SerializeField] private Text squatUpgradeCost;

    [SerializeField] private Text boxingLevel;
    [SerializeField] private Text runningLevel;
    [SerializeField] private Text squatLevel;

    private DataManager dataManager;

    private void UpdateUIForBoxing()
    {
        boxingFill.fillAmount = (float)dataManager.CurrentInteractableData.BoxingLevel / (dataManager.ListInteractableTable.InteractableTables[0].Levels.Count - 1);

        //
        if (dataManager.CurrentInteractableData.BoxingLevel != dataManager.ListInteractableTable.InteractableTables[0].Levels.Count - 1)
        {
            boxingUpgradeCost.text = dataManager.ListInteractableTable.InteractableTables[0].Levels[dataManager.CurrentInteractableData.BoxingLevel].Cost.ToString();
            boxingLevel.text = $"Level {dataManager.CurrentInteractableData.BoxingLevel}";
        }
        else
        {
            boxingUpgradeCost.text = $"Max Level";
            boxingLevel.text = $"Level {dataManager.CurrentInteractableData.BoxingLevel}";
        }
    }

    private void UpdateUIForRunning()
    {
        runningFill.fillAmount = (float)dataManager.CurrentInteractableData.RunningLevel / (dataManager.ListInteractableTable.InteractableTables[1].Levels.Count - 1);

        //
        if (dataManager.CurrentInteractableData.RunningLevel != dataManager.ListInteractableTable.InteractableTables[1].Levels.Count - 1)
        {
            runningUpgradeCost.text = dataManager.ListInteractableTable.InteractableTables[1].Levels[dataManager.CurrentInteractableData.RunningLevel].Cost.ToString();
            runningLevel.text = $"Level {dataManager.CurrentInteractableData.RunningLevel}";
        }
        else
        {
            runningUpgradeCost.text = $"Max Level";
            runningLevel.text = $"Level {dataManager.CurrentInteractableData.RunningLevel}";
        }
    }

    private void UpdateUIForSquat()
    {
        squatFill.fillAmount = (float)dataManager.CurrentInteractableData.SquatLevel / (dataManager.ListInteractableTable.InteractableTables[2].Levels.Count - 1);

        //
        if (dataManager.CurrentInteractableData.SquatLevel != dataManager.ListInteractableTable.InteractableTables[2].Levels.Count - 1)
        {
            squatUpgradeCost.text = dataManager.ListInteractableTable.InteractableTables[2].Levels[dataManager.CurrentInteractableData.SquatLevel].Cost.ToString();
            squatLevel.text = $"Level {dataManager.CurrentInteractableData.SquatLevel}";
        }
        else
        {
            squatUpgradeCost.text = $"Max Level";
            squatLevel.text = $"Level {dataManager.CurrentInteractableData.SquatLevel}";
        }
    }

    private void UpdateUIForShop()
    {
        UpdateUIForBoxing();
        UpdateUIForRunning();
        UpdateUIForSquat();
    }

    #endregion
}
