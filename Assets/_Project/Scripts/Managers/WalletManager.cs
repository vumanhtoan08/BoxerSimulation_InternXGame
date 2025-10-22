using UnityEngine;

public class WalletManager : Singleton<WalletManager>
{
    [SerializeField] private WalletData dataRuntime; 

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
        DataManager.Instance.SaveData();
    }

    #endregion
}
