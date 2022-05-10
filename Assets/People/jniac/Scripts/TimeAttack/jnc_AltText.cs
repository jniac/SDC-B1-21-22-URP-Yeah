using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class jnc_AltText : MonoBehaviour
{
    [TextArea(3, 10)]
    public string altText = "";

    public void ChangeText()
    {
        var text = GetComponent<TMPro.TextMeshProUGUI>();
        string tmp = text.text;
        text.text = altText;
        altText = tmp;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(jnc_AltText))]
    class MyEditor : Editor
    {
        jnc_AltText Target => target as jnc_AltText;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Swap"))
            {
                Undo.RecordObject(Target, "Swap");
                Target.ChangeText();
            }
        }
    }
#endif
}
