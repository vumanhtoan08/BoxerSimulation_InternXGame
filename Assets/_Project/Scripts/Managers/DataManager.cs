using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private const string PlayerDataKey = "PlayerData";
    private const string WalletDataKey = "WalletData";
    private const string InteractableDataKey = "InteractableData";

    [SerializeField] private ListStatLevelTable statLevelTables;
    [SerializeField] private ListSkillLevelTable skillLevelTables;
    [SerializeField] private EnemyStatDatabase enemyStatDatabase;
    [SerializeField] private ListInteractableTable listInteractableTable;

    public ListStatLevelTable StatLevelTables => statLevelTables;
    public ListSkillLevelTable SkillLevelTables => skillLevelTables;
    public EnemyStatDatabase EnemyStatDatabase => enemyStatDatabase;
    public ListInteractableTable ListInteractableTable => listInteractableTable;

    public DataSaveForPlayer CurrentPlayerData { get; private set; }
    public DataSaveForWallet CurrentWalletData { get; private set; }
    public DataSaveForInteractable CurrentInteractableData { get; private set; }

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();
        LoadData();
    }

    public void OnStart()
    {

    }

    public void OnUpdate()
    {

    }

    #endregion

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(PlayerDataKey))
        {
            // Đã có dữ liệu -> load từ PlayerPrefs
            string json = PlayerPrefs.GetString(PlayerDataKey);
            CurrentPlayerData = JsonUtility.FromJson<DataSaveForPlayer>(json);
            Debug.Log("✅ Player data loaded from PlayerPrefs");
        }
        else
        {
            // Chưa có dữ liệu -> tạo mới và lưu mặc định
            CurrentPlayerData = new DataSaveForPlayer();
            SaveData();
            Debug.Log("⚙️ No player data found. Created default values.");
        }

        if (PlayerPrefs.HasKey(WalletDataKey))
        {
            string json = PlayerPrefs.GetString(WalletDataKey);
            CurrentWalletData = JsonUtility.FromJson<DataSaveForWallet>(json);
            Debug.Log("✅ Wallet data loaded from PlayerPrefs");
        }
        else
        {
            CurrentWalletData = new DataSaveForWallet();
            SaveData();
            Debug.Log("⚙️ No wallet data found. Created default values.");
        }

        if (PlayerPrefs.HasKey(InteractableDataKey))
        {
            string json = PlayerPrefs.GetString(InteractableDataKey);
            CurrentInteractableData = JsonUtility.FromJson<DataSaveForInteractable>(json);
            Debug.Log("✅ Interactable data loaded from PlayerPrefs");
        }
        else
        {
            CurrentInteractableData = new DataSaveForInteractable();
            SaveData();
            Debug.Log("⚙️ No Interactable data found. Created default values.");
        }
    }

    public void SaveData()
    {
        string jsonPlayerData = JsonUtility.ToJson(CurrentPlayerData);
        PlayerPrefs.SetString(PlayerDataKey, jsonPlayerData);

        string jsonWalletData = JsonUtility.ToJson(CurrentWalletData);
        PlayerPrefs.SetString(WalletDataKey,jsonWalletData);

        string jsonInteractableData = JsonUtility.ToJson(CurrentInteractableData);
        PlayerPrefs.SetString(InteractableDataKey, jsonInteractableData);

        PlayerPrefs.Save();
        Debug.Log("💾 Data saved to PlayerPrefs");
    }

    private void ReloadPref()
    {
        PlayerPrefs.DeleteKey(PlayerDataKey);
        PlayerPrefs.DeleteKey(WalletDataKey);
        LoadData();
        Debug.Log("♻️ Player data reset to default");
    }

    internal void ChangeGameState(Game_State battle)
    {
        throw new NotImplementedException();
    }
}

#region PlayerData

[System.Serializable]
public class PlayerData
{
    [Header("Base Stats")]
    public float Attack;
    public float Health;
    public float Stamina;

    [Header("Training Process")]
    public int AttackLevel;             //
    public int CurrentAttackProcess;    // 
    public int MaxAttackProcess;
    public int HealthLevel;             //
    public int CurrentHealthProcess;    //
    public int MaxHealthProcess;
    public int StaminaLevel;            //
    public int CurrentStaminaProcess;   // 
    public int MaxStaminaProcess;

    [Header("Cost Stamina")]
    public float PunchCost;
    public float CounterCost;
    public float BlockCost;

    public void SetDataForPlayer()
    {
        Attack = DataManager.Instance.StatLevelTables.StatLevelTables[0].Levels[DataManager.Instance.CurrentPlayerData.AttackLevel].Value;
        Health = DataManager.Instance.StatLevelTables.StatLevelTables[2].Levels[DataManager.Instance.CurrentPlayerData.HealthLevel].Value;
        Stamina = DataManager.Instance.StatLevelTables.StatLevelTables[1].Levels[DataManager.Instance.CurrentPlayerData.StaminaLevel].Value;

        AttackLevel = DataManager.Instance.CurrentPlayerData.AttackLevel;
        HealthLevel = DataManager.Instance.CurrentPlayerData.HealthLevel;
        StaminaLevel = DataManager.Instance.CurrentPlayerData.StaminaLevel;

        CurrentAttackProcess = DataManager.Instance.CurrentPlayerData.AttackProcess;
        CurrentHealthProcess = DataManager.Instance.CurrentPlayerData.HealthProcess;
        CurrentStaminaProcess = DataManager.Instance.CurrentPlayerData.StaminaProcess;

        MaxAttackProcess = DataManager.Instance.StatLevelTables.StatLevelTables[0].Levels[DataManager.Instance.CurrentPlayerData.AttackLevel].ProgressToNext;
        MaxHealthProcess = DataManager.Instance.StatLevelTables.StatLevelTables[1].Levels[DataManager.Instance.CurrentPlayerData.AttackLevel].ProgressToNext;
        MaxStaminaProcess = DataManager.Instance.StatLevelTables.StatLevelTables[2].Levels[DataManager.Instance.CurrentPlayerData.AttackLevel].ProgressToNext;

        PunchCost = DataManager.Instance.SkillLevelTables.StatLevelTables[0].Levels[DataManager.Instance.CurrentPlayerData.AttackLevel].Cost;
        CounterCost = DataManager.Instance.SkillLevelTables.StatLevelTables[0].Levels[DataManager.Instance.CurrentPlayerData.AttackLevel].Cost;
        BlockCost = DataManager.Instance.SkillLevelTables.StatLevelTables[2].Levels[DataManager.Instance.CurrentPlayerData.HealthLevel].Cost;
    }
}

[SerializeField]
public class DataSaveForPlayer
{
    public int AttackLevel;
    public int HealthLevel;
    public int StaminaLevel;
    public int AttackProcess;
    public int HealthProcess;
    public int StaminaProcess;

    public DataSaveForPlayer()
    {
        AttackLevel = 0;
        HealthLevel = 0;
        StaminaLevel = 0;
        AttackProcess = 0;
        HealthProcess = 0;
        StaminaProcess = 0;
    }
}

#endregion

#region Stat Data 
[CreateAssetMenu(fileName = "NewStatLevelTable", menuName = "Database/ListStat")]
public class ListStatLevelTable : ScriptableObject
{
    public List<StatLevelTable> StatLevelTables = new List<StatLevelTable>();
}

[CreateAssetMenu(fileName = "NewSkillLevelTable", menuName = "Database/ListSkill")]
public class ListSkillLevelTable : ScriptableObject
{
    public List<SkillLevelTable> StatLevelTables = new List<SkillLevelTable>();
}

#endregion

#region Enemy Runtime Data

[Serializable]
public class EnemyData
{
    public float Attack;
    public float Health;

    public void SetDataForEnemy()
    {
        Attack = DataManager.Instance.EnemyStatDatabase.Enemies[0].Attack; // Ve sau thay 0 = level luu trong Prefabs
        Health = DataManager.Instance.EnemyStatDatabase.Enemies[0].Defense;
    }
}

#endregion

#region Wallet Data

[Serializable]
public class WalletData
{
    public int currentMoney;

    public void SetDataForWallet()
    {
        currentMoney = DataManager.Instance.CurrentWalletData.currentMoney;
    }
}

[SerializeField]
public class DataSaveForWallet
{
    public int currentMoney;

    public DataSaveForWallet()
    {
        currentMoney = 0;
    }
}

#endregion

#region InteractableData

[CreateAssetMenu(fileName = "NewInteractableTable", menuName = "Database/Interactable Table")]
public class InteractableTable : ScriptableObject
{
    public string ID;
    public string Name;
    public List<InteractableLevelData> Levels = new List<InteractableLevelData>();

    [System.Serializable]
    public class InteractableLevelData
    {
        public int Level;
        public int Value;
        public int Cost;
    }
}

[CreateAssetMenu(fileName = "NewInteractableTable", menuName = "Database/Interactable List")]
public class ListInteractableTable : ScriptableObject
{
    public List<InteractableTable> InteractableTables = new List<InteractableTable>();
}

[SerializeField]
public class DataSaveForInteractable
{
    public int BoxingLevel;
    public int RunningLevel;
    public int SquatLevel;

    public DataSaveForInteractable()
    {
        BoxingLevel = 0;
        RunningLevel = 0;
        SquatLevel = 0;
    }
}

[Serializable]
public class InteractableData
{
    public int Level;
    public int Value;
    public int Cost;

    public void SetDataForInteractable(TYPE_TRAINING type)
    {
        switch (type)
        {
            case TYPE_TRAINING.NONE:
                break;
            case TYPE_TRAINING.BOXING:
                Level = DataManager.Instance.ListInteractableTable.InteractableTables[0].Levels[DataManager.Instance.CurrentInteractableData.BoxingLevel].Level;
                Value = DataManager.Instance.ListInteractableTable.InteractableTables[0].Levels[DataManager.Instance.CurrentInteractableData.BoxingLevel].Value;
                Cost = DataManager.Instance.ListInteractableTable.InteractableTables[0].Levels[DataManager.Instance.CurrentInteractableData.BoxingLevel].Cost;
                break;
            case TYPE_TRAINING.RUNING:
                Level = DataManager.Instance.ListInteractableTable.InteractableTables[1].Levels[DataManager.Instance.CurrentInteractableData.RunningLevel].Level;
                Value = DataManager.Instance.ListInteractableTable.InteractableTables[1].Levels[DataManager.Instance.CurrentInteractableData.RunningLevel].Value;
                Cost = DataManager.Instance.ListInteractableTable.InteractableTables[1].Levels[DataManager.Instance.CurrentInteractableData.RunningLevel].Cost;
                break;
            case TYPE_TRAINING.SQUAT:
                Level = DataManager.Instance.ListInteractableTable.InteractableTables[2].Levels[DataManager.Instance.CurrentInteractableData.SquatLevel].Level;
                Value = DataManager.Instance.ListInteractableTable.InteractableTables[2].Levels[DataManager.Instance.CurrentInteractableData.SquatLevel].Value;
                Cost = DataManager.Instance.ListInteractableTable.InteractableTables[2].Levels[DataManager.Instance.CurrentInteractableData.SquatLevel].Cost;
                break;
            default:
                break;
        }
    }
}

#endregion