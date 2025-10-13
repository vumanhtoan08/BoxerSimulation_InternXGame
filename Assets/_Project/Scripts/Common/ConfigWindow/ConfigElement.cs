using System;
using System.Collections;
using System.Collections.Generic;
using DX.CF;
using UnityEngine;
using UnityEngine.UI;

namespace DX.CF
{
    public class ConfigElement : MonoBehaviour
    {
        [SerializeField] Text keyText;
        [SerializeField] InputField inputField;
        [SerializeField] Dropdown dropdown;
        private ConfigWindow.ConfigInfoData configInfo;
        private ConfigWindow configWindow;
        public void Initialize(ConfigWindow configWindow)
        {
            this.configWindow = configWindow;
            inputField.onEndEdit.AddListener(OnEndEdit);
            dropdown.onValueChanged.AddListener(OnChange);
            gameObject.SetActive(true);
        }
        public void SetupData(ConfigWindow.ConfigInfoData configInfo)
        {
            this.configInfo = configInfo;
            keyText.text = configInfo.key;
            dropdown.value = configInfo.type;
            inputField.text = configInfo.value;
        }

        private void OnChange(int arg0)
        {
            configInfo.type = arg0;
            configWindow.ModifyConfig(configInfo);
        }

        private void OnEndEdit(string arg0)
        {
            configInfo.value = arg0;
            configWindow.ModifyConfig(configInfo);
        }
    }
}
