using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace Common
{

    public class ScriptableSettingsProxy : ScriptableObject
    {

        public ScriptableObject target;

#if UNITY_EDITOR

        [CustomEditor(typeof(ScriptableSettingsProxy))]
        public class Editor : UnityEditor.Editor
        {

            UnityEditor.Editor editor;
            private void OnEnable()
            {
                editor = CreateEditor(serializedObject.FindProperty(nameof(target)).objectReferenceValue);
            }

            protected override void OnHeaderGUI()
            {
                editor.DrawHeader();
            }

            public override void OnInspectorGUI()
            {
                editor.OnInspectorGUI();
            }

        }

#endif

    }
}
