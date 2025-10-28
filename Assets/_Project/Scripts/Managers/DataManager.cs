using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;



public class DataManager : Singleton<DataManager>
{
    private const string PlayerDataKey = "PlayerData";
    private const string WalletDataKey = "WalletData";
    private const string InteractableDataKey = "InteractableData";
    private const string DayDataKey = "DayData";
    private const string EnemyDataKey = "EnemyData";

    [SerializeField] private ListStatLevelTable statLevelTables;
    [SerializeField] private ListSkillLevelTable skillLevelTables;
    [SerializeField] private EnemyStatDatabase enemyStatDatabase;
    [SerializeField] private ListInteractableTable listInteractableTable;

    public ListStatLevelTable StatLevelTables => statLevelTables;
    public ListSkillLevelTable SkillLevelTables => skillLevelTables;
    public EnemyStatDatabase EnemyStatDatabase => enemyStatDatabase;
    public ListInteractableTable ListInteractableTable => listInteractableTable;

    public DataSaveForPlayer CurrentPlayerData { get; private set; }                       // 3 cái này lấy từ PlayerPrebs
    public DataSaveForWallet CurrentWalletData { get; private set; }                       // 3 cái này lấy từ PlayerPrebs
    public DataSaveForInteractable CurrentInteractableData { get; private set; }           // 3 cái này lấy từ PlayerPrebs
    public DataSaveForDay CurrentDayData { get; private set; }
    public DataSaveForEnemy CurrentEnemyData { get; private set; }

    #region Unity Methods

    public void OnAwake()
    {
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
        // --- PLAYER ---
        if (PlayerPrefs.HasKey(PlayerDataKey))
        {
            string json = PlayerPrefs.GetString(PlayerDataKey);
            if (!string.IsNullOrEmpty(json))
                CurrentPlayerData = JsonUtility.FromJson<DataSaveForPlayer>(json);
            else
                CurrentPlayerData = new DataSaveForPlayer();

            Debug.Log("✅ Player data loaded from PlayerPrefs");
        }
        else
        {
            CurrentPlayerData = new DataSaveForPlayer();
            Debug.Log("⚙️ No player data found. Created default values.");
        }

        // --- WALLET ---
        if (PlayerPrefs.HasKey(WalletDataKey))
        {
            string json = PlayerPrefs.GetString(WalletDataKey);
            if (!string.IsNullOrEmpty(json))
                CurrentWalletData = JsonUtility.FromJson<DataSaveForWallet>(json);
            else
                CurrentWalletData = new DataSaveForWallet();

            Debug.Log("✅ Wallet data loaded from PlayerPrefs");
        }
        else
        {
            CurrentWalletData = new DataSaveForWallet();
            Debug.Log("⚙️ No wallet data found. Created default values.");
        }

        // --- INTERACTABLE ---
        if (PlayerPrefs.HasKey(InteractableDataKey))
        {
            string json = PlayerPrefs.GetString(InteractableDataKey);
            if (!string.IsNullOrEmpty(json))
                CurrentInteractableData = JsonUtility.FromJson<DataSaveForInteractable>(json);
            else
                CurrentInteractableData = new DataSaveForInteractable();

            Debug.Log("✅ Interactable data loaded from PlayerPrefs");
        }
        else
        {
            CurrentInteractableData = new DataSaveForInteractable();
            Debug.Log("⚙️ No Interactable data found. Created default values.");
        }

        // --- DAY ---
        if (PlayerPrefs.HasKey(DayDataKey))
        {
            string json = PlayerPrefs.GetString(DayDataKey);
            if (!string.IsNullOrEmpty(json))
                CurrentDayData = JsonUtility.FromJson<DataSaveForDay>(json);
            else
                CurrentDayData = new DataSaveForDay();

            Debug.Log("✅ Day data loaded from PlayerPrefs");
        }
        else
        {
            CurrentDayData = new DataSaveForDay();
            Debug.Log("⚙️ No day data found. Created default values.");
        }

        // --- ENEMY ---
        if (PlayerPrefs.HasKey(EnemyDataKey))
        {
            string json = PlayerPrefs.GetString(EnemyDataKey);
            if (!string.IsNullOrEmpty(json))
                CurrentEnemyData = JsonUtility.FromJson<DataSaveForEnemy>(json);
            else
                CurrentEnemyData = new DataSaveForEnemy();

            Debug.Log("✅ Enemy data loaded from PlayerPrefs");
        }
        else
        {
            CurrentEnemyData = new DataSaveForEnemy();
            Debug.Log("⚙️ No enemy data found. Created default values.");
        }

        // 🔥 Đảm bảo tất cả đã được khởi tạo trước khi SaveData()
        if (CurrentPlayerData == null) CurrentPlayerData = new DataSaveForPlayer();
        if (CurrentWalletData == null) CurrentWalletData = new DataSaveForWallet();
        if (CurrentInteractableData == null) CurrentInteractableData = new DataSaveForInteractable();
        if (CurrentDayData == null) CurrentDayData = new DataSaveForDay();
        if (CurrentEnemyData == null) CurrentEnemyData = new DataSaveForEnemy();

        SaveData(); // ✅ Gọi 1 lần duy nhất, sau khi tất cả có dữ liệu
    }


    public void SaveData()
    {
        string jsonPlayerData = JsonUtility.ToJson(CurrentPlayerData);
        PlayerPrefs.SetString(PlayerDataKey, jsonPlayerData);

        string jsonWalletData = JsonUtility.ToJson(CurrentWalletData);
        PlayerPrefs.SetString(WalletDataKey, jsonWalletData);

        string jsonInteractableData = JsonUtility.ToJson(CurrentInteractableData);
        PlayerPrefs.SetString(InteractableDataKey, jsonInteractableData);

        string jsonDayData = JsonUtility.ToJson(CurrentDayData);
        PlayerPrefs.SetString(DayDataKey, jsonDayData);

        string jsonEnemyData = JsonUtility.ToJson(CurrentEnemyData);
        PlayerPrefs.SetString(EnemyDataKey, jsonEnemyData);

        PlayerPrefs.Save();
        Debug.Log("💾 Data saved to PlayerPrefs");
    }

    private void ReloadPref()
    {
        PlayerPrefs.DeleteKey(PlayerDataKey);
        PlayerPrefs.DeleteKey(WalletDataKey);
        PlayerPrefs.DeleteKey(InteractableDataKey);
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
        MaxHealthProcess = DataManager.Instance.StatLevelTables.StatLevelTables[2].Levels[DataManager.Instance.CurrentPlayerData.HealthLevel].ProgressToNext;
        MaxStaminaProcess = DataManager.Instance.StatLevelTables.StatLevelTables[1].Levels[DataManager.Instance.CurrentPlayerData.StaminaLevel].ProgressToNext;

        PunchCost = DataManager.Instance.SkillLevelTables.StatLevelTables[0].Levels[DataManager.Instance.CurrentPlayerData.AttackLevel].Cost;
        CounterCost = DataManager.Instance.SkillLevelTables.StatLevelTables[0].Levels[DataManager.Instance.CurrentPlayerData.AttackLevel].Cost;
        BlockCost = DataManager.Instance.SkillLevelTables.StatLevelTables[2].Levels[DataManager.Instance.CurrentPlayerData.HealthLevel].Cost;
    }

    public void UpgradeState(STATE_TYPE type)
    {
        var dataManager = DataManager.Instance;
        var playerData = dataManager.CurrentPlayerData;
        var statTables = dataManager.StatLevelTables.StatLevelTables;

        switch (type)
        {
            case STATE_TYPE.Attack:
                {
                    int maxLevel = statTables[0].Levels.Count - 1;
                    if (playerData.AttackLevel >= maxLevel)
                    {
                        Debug.LogWarning($"⚠️ Attack đã đạt cấp tối đa ({maxLevel})!");
                        return;
                    }

                    playerData.AttackLevel++;
                    dataManager.SaveData();
                    SetDataForPlayer();
                    Debug.Log($"✅ Attack upgraded to level {playerData.AttackLevel}");
                    break;
                }

            case STATE_TYPE.Health:
                {
                    int maxLevel = statTables[2].Levels.Count - 1;
                    if (playerData.HealthLevel >= maxLevel)
                    {
                        Debug.LogWarning($"⚠️ Health đã đạt cấp tối đa ({maxLevel})!");
                        return;
                    }

                    playerData.HealthLevel++;
                    dataManager.SaveData();
                    SetDataForPlayer();
                    Debug.Log($"✅ Health upgraded to level {playerData.HealthLevel}");
                    break;
                }

            case STATE_TYPE.Stamina:
                {
                    int maxLevel = statTables[1].Levels.Count - 1;
                    if (playerData.StaminaLevel >= maxLevel)
                    {
                        Debug.LogWarning($"⚠️ Stamina đã đạt cấp tối đa ({maxLevel})!");
                        return;
                    }

                    playerData.StaminaLevel++;
                    dataManager.SaveData();
                    SetDataForPlayer();
                    Debug.Log($"✅ Stamina upgraded to level {playerData.StaminaLevel}");
                    break;
                }

            default:
                Debug.LogWarning("⚠️ STATE_TYPE không hợp lệ khi gọi UpgradeState()");
                break;
        }
    }

    public void UpProcess(TYPE_TRAINING type)
    {
        var dataManager = DataManager.Instance;
        var playerData = dataManager.CurrentPlayerData;

        switch (type)
        {
            case TYPE_TRAINING.BOXING:
                {
                    int currentLevel = playerData.AttackLevel;
                    int maxLevel = dataManager.StatLevelTables.StatLevelTables[0].Levels.Count - 1;

                    if (currentLevel >= maxLevel)
                    {
                        Debug.LogWarning("🥊 Boxing đã đạt cấp tối đa!");
                        return;
                    }

                    playerData.AttackProcess++;
                    DataManager.Instance.CurrentPlayerData.AttackProcess = playerData.AttackProcess;
                    Debug.Log($"➡️ Process Boxing: {playerData.AttackProcess}/{MaxAttackProcess} == {DataManager.Instance.CurrentPlayerData.AttackProcess}");

                    dataManager.SaveData();
                    SetDataForPlayer();

                    if (playerData.AttackProcess >= MaxAttackProcess)
                    {
                        playerData.AttackProcess = 0;
                        DataManager.Instance.CurrentPlayerData.AttackProcess = 0;
                        UpgradeState(STATE_TYPE.Attack);
                    }

                    break;
                }

            case TYPE_TRAINING.RUNING:
                {
                    int currentLevel = playerData.StaminaLevel;
                    int maxLevel = dataManager.StatLevelTables.StatLevelTables[1].Levels.Count - 1;

                    if (currentLevel >= maxLevel)
                    {
                        Debug.LogWarning("🏃‍♂️ Running đã đạt cấp tối đa!");
                        return;
                    }

                    playerData.StaminaProcess++;
                    DataManager.Instance.CurrentPlayerData.StaminaProcess = playerData.StaminaProcess;
                    Debug.Log($"➡️ Process Running: {playerData.StaminaProcess}/{MaxStaminaProcess}");

                    dataManager.SaveData();
                    SetDataForPlayer();

                    if (playerData.StaminaProcess >= MaxStaminaProcess)
                    {
                        playerData.StaminaProcess = 0;
                        DataManager.Instance.CurrentPlayerData.StaminaProcess = 0;
                        UpgradeState(STATE_TYPE.Stamina);
                    }

                    break;
                }

            case TYPE_TRAINING.SQUAT:
                {
                    int currentLevel = playerData.HealthLevel;
                    int maxLevel = dataManager.StatLevelTables.StatLevelTables[2].Levels.Count - 1;

                    if (currentLevel >= maxLevel)
                    {
                        Debug.LogWarning("🏋️‍♂️ Squat đã đạt cấp tối đa!");
                        return;
                    }

                    playerData.HealthProcess++;
                    DataManager.Instance.CurrentPlayerData.HealthProcess = playerData.HealthProcess;
                    Debug.Log($"➡️ Process Squat: {playerData.HealthProcess}/{MaxHealthProcess}");

                    dataManager.SaveData();
                    SetDataForPlayer();

                    if (playerData.HealthProcess >= MaxHealthProcess)
                    {
                        playerData.HealthProcess = 0;
                        DataManager.Instance.CurrentPlayerData.HealthProcess = 0;
                        UpgradeState(STATE_TYPE.Health);
                    }

                    break;
                }

            default:
                Debug.LogWarning("⚠️ Loại tập luyện không hợp lệ!");
                break;
        }
    }
}

public enum STATE_TYPE
{
    Attack,
    Health,
    Stamina,
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
    public string Name;
    public float Attack;
    public float Health;
    public int Reward;

    public void SetDataForEnemy()
    {
        Name = DataManager.Instance.EnemyStatDatabase.Enemies[DataManager.Instance.CurrentEnemyData.Level].Name;
        Attack = DataManager.Instance.EnemyStatDatabase.Enemies[DataManager.Instance.CurrentEnemyData.Level].Attack; // Ve sau thay 0 = level luu trong Prefabs
        Health = DataManager.Instance.EnemyStatDatabase.Enemies[DataManager.Instance.CurrentEnemyData.Level].Defense;
        Reward = DataManager.Instance.EnemyStatDatabase.Enemies[DataManager.Instance.CurrentEnemyData.Level].Money;
    }
}

[Serializable]
public class DataSaveForEnemy
{
    public int Level;

    public DataSaveForEnemy() { Level = 0; }
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

#region Day Data 

[Serializable]
public class DayData
{
    public int currentDay;

    public void SetDataForDay()
    {
        currentDay = DataManager.Instance.CurrentDayData.currentDay;
    }
}

[Serializable]
public class DataSaveForDay
{
    public int currentDay;

    public DataSaveForDay()
    {
        currentDay = 1;
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

    public void UpgradeInteractable(TYPE_TRAINING type)
    {
        var data = DataManager.Instance.CurrentInteractableData;
        var tables = DataManager.Instance.ListInteractableTable.InteractableTables;

        switch (type)
        {
            case TYPE_TRAINING.NONE:
                return;

            case TYPE_TRAINING.BOXING:
                {
                    int maxLevel = tables[0].Levels.Count - 1;
                    if (data.BoxingLevel >= maxLevel)
                    {
                        Debug.LogWarning($"⚠️ {type} đã đạt cấp tối đa ({maxLevel})!");
                        return;
                    }

                    data.BoxingLevel++;
                    DataManager.Instance.SaveData();
                    break;
                }

            case TYPE_TRAINING.RUNING:
                {
                    int maxLevel = tables[1].Levels.Count - 1;
                    if (data.RunningLevel >= maxLevel)
                    {
                        Debug.LogWarning($"⚠️ {type} đã đạt cấp tối đa ({maxLevel})!");
                        return;
                    }

                    data.RunningLevel++;
                    DataManager.Instance.SaveData();
                    break;
                }

            case TYPE_TRAINING.SQUAT:
                {
                    int maxLevel = tables[2].Levels.Count - 1;
                    if (data.SquatLevel >= maxLevel)
                    {
                        Debug.LogWarning($"⚠️ {type} đã đạt cấp tối đa ({maxLevel})!");
                        return;
                    }

                    data.SquatLevel++;
                    DataManager.Instance.SaveData();
                    break;
                }
        }

        SetDataForInteractable(type);

        Debug.Log($"✅ {type} upgraded! (Level: {Level})");
    }
}

#endregion