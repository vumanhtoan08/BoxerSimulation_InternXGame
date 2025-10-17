using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
namespace D.Editor
{

//#if UNITY_EDITOR

//    using UnityEditor;

//    [CustomEditor(typeof(DataManager))]
//    public class DataManagerEditor : Editor
//    {
//        public Dictionary<string, string> prefInfos;

//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();
//            if (prefInfos == null)
//            {
//                foldOut = true;
//                if (GUILayout.Button("Show Prefs"))
//                {
//                    string data = SaveSystem.LoadGame(DataManager.DATAPATH);
//                    if (data == null || data.Length == 0) return;
//                    var userData = JsonConvert.DeserializeObject<PlayerData>(data);
//                    //if (userData.prefData == null || userData.prefData.Length == 0) return;
//                    //prefInfos = JsonConvert.DeserializeObject<Dictionary<string, string>>(userData.prefData);
//                }
//            }
//            else
//            {
//                ShowPrefs();
//                if (GUILayout.Button("Save Prefs"))
//                {
//                    string data = SaveSystem.LoadGame(DataManager.DATAPATH);
//                    if (data == null || data.Length == 0) return;
//                    var userData = JsonConvert.DeserializeObject<PlayerData>(data);
//                    string newData = JsonConvert.SerializeObject(prefInfos);
//                    //userData.prefData = newData;
//                    var d = JsonConvert.SerializeObject(userData);
//                    PlayerPrefs.SetString("user_data", d);
//                    Debug.Log("Saved:" + d);
//                    if (Application.isPlaying)
//                    {
//                        DataManager dataManager = target as DataManager;
//                        //dataManager.data.prefData = newData;
//                        dataManager.SendMessage("ReloadPref");
//                    }
//                }
//            }
//        }

//        private bool foldOut;

//        private void ShowPrefs()
//        {
//            int index = 0;
//            foldOut = EditorGUILayout.Foldout(foldOut, "ListPrefs");
//            if (foldOut)
//            {
//                EditorGUI.indentLevel++;
//                var newList = new Dictionary<string, string>(prefInfos);
//                foreach (var item in newList)
//                {
//                    EditorGUILayout.LabelField("Element " + index);
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.BeginHorizontal();
//                    EditorGUILayout.LabelField(item.Key);
//                    string newValue = "";
//                    if (int.TryParse(item.Value, out int newNum))
//                    {
//                        newValue = EditorGUILayout.IntField(newNum).ToString();
//                    }
//                    else
//                    {
//                        newValue = EditorGUILayout.TextField(item.Value);
//                    }
//                    if (newValue != item.Value)
//                    {
//                        prefInfos[item.Key] = newValue;
//                        Debug.Log("change " + item.Key + ":" + newValue);
//                    }
//                    EditorGUILayout.EndHorizontal();
//                    index++;
//                    EditorGUI.indentLevel--;
//                }
//                EditorGUI.indentLevel--;
//            }

//        }
//    }

//#endif

}
public class DataManager : Singleton<DataManager>
{
    private const string PlayerDataKey = "PlayerData";

    [SerializeField] private ListStatLevelTable statLevelTables;
    [SerializeField] private ListSkillLevelTable skillLevelTables; 
    [SerializeField] private EnemyStatDatabase enemyStatDatabase;


    public ListStatLevelTable StatLevelTables => statLevelTables; 
    public ListSkillLevelTable SkillLevelTables => skillLevelTables; 

    public DataSaveForPlayer CurrentPlayerData { get; private set; }

    public void Awake()
    {
        LoadData();
    }

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
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(CurrentPlayerData);
        PlayerPrefs.SetString(PlayerDataKey, json);
        PlayerPrefs.Save();
        Debug.Log("💾 Player data saved to PlayerPrefs");
    }

    private void ReloadPref()
    {
        PlayerPrefs.DeleteKey(PlayerDataKey);
        LoadData();
        Debug.Log("♻️ Player data reset to default");
    }
}

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
        Health = DataManager.Instance.StatLevelTables.StatLevelTables[1].Levels[DataManager.Instance.CurrentPlayerData.HealthLevel].Value;
        Stamina = DataManager.Instance.StatLevelTables.StatLevelTables[2].Levels[DataManager.Instance.CurrentPlayerData.StaminaLevel].Value;

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