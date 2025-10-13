using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(FontReplace))]
public class FontReplaceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("ChangeFont"))
        {
            FontReplace fontReplace = target as FontReplace;
            Text[] texts = FindObjectsOfType<Text>(true);
            for (int i = 0; i < texts.Length; i++)
            {
                Undo.RecordObject(texts[i], texts[i].name + "replace");
                texts[i].font = fontReplace.newFont;
                EditorUtility.SetDirty(texts[i]);
            }
        }
    }

}
#endif
public class FontReplace : MonoBehaviour
{
    public Font newFont;
}
