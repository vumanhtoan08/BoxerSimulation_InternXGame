using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
namespace D.Editor
{

#if UNITY_EDITOR

    using UnityEditor;

    [CustomEditor(typeof(DataManager))]
    public class DataManagerEditor : Editor
    {
        public Dictionary<string, string> prefInfos;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (prefInfos == null)
            {
                foldOut = true;
                if (GUILayout.Button("Show Prefs"))
                {
                    string data = SaveSystem.LoadGame(DataManager.DATAPATH);
                    if (data == null || data.Length == 0) return;
                    var userData = JsonConvert.DeserializeObject<PlayerData>(data);
                    if (userData.prefData == null || userData.prefData.Length == 0) return;
                    prefInfos = JsonConvert.DeserializeObject<Dictionary<string, string>>(userData.prefData);
                }
            }
            else
            {
                ShowPrefs();
                if (GUILayout.Button("Save Prefs"))
                {
                    string data = SaveSystem.LoadGame(DataManager.DATAPATH);
                    if (data == null || data.Length == 0) return;
                    var userData = JsonConvert.DeserializeObject<PlayerData>(data);
                    string newData = JsonConvert.SerializeObject(prefInfos);
                    userData.prefData = newData;
                    var d = JsonConvert.SerializeObject(userData);
                    PlayerPrefs.SetString("user_data", d);
                    Debug.Log("Saved:" + d);
                    if (Application.isPlaying)
                    {
                        DataManager dataManager = target as DataManager;
                        dataManager.data.prefData = newData;
                        dataManager.SendMessage("ReloadPref");
                    }
                }
            }
        }

        private bool foldOut;

        private void ShowPrefs()
        {
            int index = 0;
            foldOut = EditorGUILayout.Foldout(foldOut, "ListPrefs");
            if (foldOut)
            {
                EditorGUI.indentLevel++;
                var newList = new Dictionary<string, string>(prefInfos);
                foreach (var item in newList)
                {
                    EditorGUILayout.LabelField("Element " + index);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(item.Key);
                    string newValue = "";
                    if (int.TryParse(item.Value, out int newNum))
                    {
                        newValue = EditorGUILayout.IntField(newNum).ToString();
                    }
                    else
                    {
                        newValue = EditorGUILayout.TextField(item.Value);
                    }
                    if (newValue != item.Value)
                    {
                        prefInfos[item.Key] = newValue;
                        Debug.Log("change " + item.Key + ":" + newValue);
                    }
                    EditorGUILayout.EndHorizontal();
                    index++;
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }

        }
    }

#endif

}
public class DataManager : Singleton<DataManager>
{
    public static readonly string DATAPATH = "player_data";
    public PlayerData data;
    private Dictionary<string, string> userDicDataPref;
    void LoadData()
    {
        string myData = SaveSystem.LoadGame(DATAPATH);
        if (myData.Length > 1)
        {
            Debug.Log("Load:" + myData);
            data = JsonUtility.FromJson<PlayerData>(myData);
            if (data.prefData != null && data.prefData.Length > 0)
            {
                userDicDataPref = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.prefData);
            }
        }
        else
        {
            data = new PlayerData();
        }
    }
    void SaveData()
    {
        data.prefData = JsonConvert.SerializeObject(userDicDataPref);
        string dt = JsonConvert.SerializeObject(data);
        Debug.Log("Save:" + dt);
        SaveSystem.SaveFile(DATAPATH, dt);
    }
    private void ReloadPref()
    {
        Debug.Log("Reload");
        userDicDataPref = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.prefData);
    }

    #region Save Sync

    public string GetString(string key, string defaultValue)
    {
        if (userDicDataPref.ContainsKey(key))
        {
            return userDicDataPref[key];
        }
        else
        {
            return defaultValue;
        }
    }
    public void SetString(string key, string newValue)
    {
        if (userDicDataPref.ContainsKey(key))
        {
            userDicDataPref[key] = newValue;
        }
        else
        {
            userDicDataPref.Add(key, newValue);
        }
        SaveData();
    }
    public int GetInt(string key, int defaultValue)
    {
        if (userDicDataPref.ContainsKey(key))
        {
            int data = Convert.ToInt32(userDicDataPref[key]);
            return data;
        }
        else
        {
            return defaultValue;
        }
    }
    public void SetInt(string key, int newValue)
    {
        if (userDicDataPref.ContainsKey(key))
        {
            userDicDataPref[key] = newValue.ToString();
        }
        else
        {
            userDicDataPref.Add(key, newValue.ToString());
        }
        SaveData();
    }
    #endregion
}
[System.Serializable]
public class PlayerData
{
    public string prefData;
    public PlayerData()
    {
        prefData = "";
    }
}