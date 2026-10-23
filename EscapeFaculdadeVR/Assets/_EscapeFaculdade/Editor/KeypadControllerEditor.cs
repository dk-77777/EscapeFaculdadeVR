using UnityEditor;
using UnityEngine;

namespace EscapeFaculdade.EditorTools
{
    [CustomEditor(typeof(KeypadController))]
    public class KeypadControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(
                "Teste em Play Mode: estes botões chamam os KeypadButton da hierarquia. " +
                "Eles validam a lógica do puzzle; teste a seleção e a locomoção também no headset.",
                MessageType.Info);

            using (new EditorGUI.DisabledScope(!Application.isPlaying))
            {
                KeypadButton[] keys = ((KeypadController)target).GetComponentsInChildren<KeypadButton>();
                DrawRow(keys, "1", "2", "3");
                DrawRow(keys, "4", "5", "6");
                DrawRow(keys, "7", "8", "9");
                DrawRow(keys, "LIM", "0", "OK");
            }
        }

        private void DrawRow(KeypadButton[] keys, params string[] labels)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                foreach (string label in labels)
                {
                    KeypadButton key = FindKey(keys, label);
                    using (new EditorGUI.DisabledScope(key == null))
                    {
                        if (GUILayout.Button(label) && key != null)
                            key.Press();
                    }
                }
            }
        }

        private KeypadButton FindKey(KeypadButton[] keys, string label)
        {
            foreach (KeypadButton key in keys)
            {
                SerializedObject settings = new SerializedObject(key);
                if (settings.FindProperty("keypad").objectReferenceValue != target)
                    continue;

                KeypadButton.ButtonType type = (KeypadButton.ButtonType)settings.FindProperty("buttonType").enumValueIndex;
                if ((label == "LIM" && type == KeypadButton.ButtonType.Clear) ||
                    (label == "OK" && type == KeypadButton.ButtonType.Submit) ||
                    (type == KeypadButton.ButtonType.Digit && settings.FindProperty("digit").stringValue == label))
                    return key;
            }

            return null;
        }
    }
}
