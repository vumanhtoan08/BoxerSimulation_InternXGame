using System;
using UnityEngine;

public class WalletManager : Singleton<WalletManager>
{
    [SerializeField] private WalletData dataRuntime;

    public WalletData DataRuntime => dataRuntime;

    public event Action OnDataChange;

    #region Unity Methods

    public void OnStart()
    {
        dataRuntime.SetDataForWallet();
    }

    public void OnUpdate()
    {

    }

    #endregion

    #region ChangeMoney Methods

    public void OnMoneyChange(int value)
    {
        dataRuntime.currentMoney = Mathf.Max(0, dataRuntime.currentMoney + value);

        DataManager.Instance.CurrentWalletData.currentMoney = dataRuntime.currentMoney;
        DataManager.Instance.SaveData();

        OnDataChange?.Invoke();
    }

    #endregion
}
