using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace DX.CF
{
    public class ConfigWindow : MonoBehaviour
    {
        public EditButton editButton;
        public GameObject rootObject;
        public CanvasScaler canvasScaler;
        private bool isOpen;
        private readonly string fileName = "config_dx_zd_dy.ex";
        private List<ConfigInfoData> configs;
        private List<ConfigElement> listElement;
        [SerializeField] ConfigElement prefabElement;
        [SerializeField] RectTransform holder;
        [Space]
        [SerializeField] InputField keyField;
        [SerializeField] InputField valueField;
        [SerializeField] Dropdown typeSelect;
        [SerializeField] Button addButton;
        [SerializeField] Text notifyText;
        public ConfigInfoData currentInfo;

        void Start()
        {
            isOpen = false;
            rootObject.SetActive(false);
            editButton.onClick.AddListener(OpenWindow);
            keyField.onEndEdit.AddListener(KeyEdit);
            valueField.onEndEdit.AddListener(ValueEdit);
            typeSelect.onValueChanged.AddListener(TypeSelect);
            addButton.onClick.AddListener(CreateConfig);
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            editButton.rectTransform.sizeDelta = screenSize * 0.12f;
            canvasScaler.referenceResolution = screenSize;
            editButton.rectTransform.anchoredPosition = new Vector2(screenSize.x - editButton.rectTransform.sizeDelta.x, screenSize.y * 0.4f);
            Load();
        }


        private void Load()
        {
            string path = Application.persistentDataPath + fileName;
            if (File.Exists(path))
            {
                string txt = File.ReadAllText(Application.persistentDataPath + fileName);
                configs = JsonConvert.DeserializeObject<List<ConfigInfoData>>(txt);
            }
            else
            {
                configs = new List<ConfigInfoData>();
            }

            listElement = new List<ConfigElement>();
            for (int i = 0; i < configs.Count; i++)
            {
                ConfigElement element = Instantiate(prefabElement, holder);
                listElement.Add(element);
                element.Initialize(this);
                element.SetupData(configs[i]);
            }
        }
        private void OpenWindow()
        {
            notifyText.text = "";
            isOpen = !isOpen;
            rootObject.SetActive(isOpen);
        }
        private void TypeSelect(int arg0)
        {
            currentInfo.type = arg0;
        }

        private void ValueEdit(string arg0)
        {
            if (currentInfo.type == 0)
            {
                if (!int.TryParse(arg0, out int x))
                {
                    notifyText.text = "<color=#FF0000> value not a number </color>";
                    return;
                }
            }
            currentInfo.value = arg0;
        }

        private void KeyEdit(string arg0)
        {
            currentInfo.key = arg0;
        }

        public void CreateConfig()
        {
            for (int i = 0; i < configs.Count; i++)
            {
                if (currentInfo.key == configs[i].key)
                {
                    notifyText.text = "<color=#FF0000>Key already exists </color>";
                    return;
                }
            }
            configs.Add(currentInfo);

            notifyText.text = "<color=#34FF00> Complete </color>";
            ConfigElement element = Instantiate(prefabElement, holder);
            listElement.Add(element);
            element.Initialize(this);
            element.SetupData(currentInfo);

            switch (currentInfo.type)
            {
                case 0:
                    PlayerPrefs.SetInt(currentInfo.key, int.Parse(currentInfo.value));
                    break;
                case 1:
                    PlayerPrefs.SetString(currentInfo.key, currentInfo.value);
                    break;
            }

            Save();
        }

        public void ModifyConfig(ConfigInfoData configInfo)
        {
            for (int i = 0; i < configs.Count; i++)
            {
                if (currentInfo.key == configs[i].key)
                {
                    var cf = configs[i];
                    cf.value = configInfo.value;
                    cf.type = configInfo.type;
                    switch (cf.type)
                    {
                        case 0:
                            PlayerPrefs.SetInt(cf.key, int.Parse(cf.value));
                            break;
                        case 1:
                            PlayerPrefs.SetString(cf.key, cf.value);
                            break;
                    }
                    break;
                }
            }
            Save();
        }

        private void Save()
        {
            var data = JsonConvert.SerializeObject(configs);
            File.WriteAllText(Application.persistentDataPath + fileName, data);
        }
        [Serializable]
        public struct ConfigInfoData
        {
            public string key;
            public string value;
            public int type;
        }
    }
}
