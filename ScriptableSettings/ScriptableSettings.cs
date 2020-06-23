using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace Common
{

    public abstract class ScriptableSettingsBase : ScriptableObject
    {

#if UNITY_EDITOR

        [CustomEditor(typeof(ScriptableSettingsBase), true, isFallback = true)]
        public class Editor : UnityEditor.Editor
        {

            public override void OnInspectorGUI()
            {
                DrawPropertiesExcluding(serializedObject, "m_Script");
            }

        }

#endif

    }

    /// <summary>A base class for creating ScriptableSettings. ScriptableSettings are automatically created and are accessible through <YourScriptableSettings>.Current or through 'Settings > YourScriptableSettings' menu item.</summary>
    public abstract class ScriptableSettings<T> : ScriptableSettingsBase where T : ScriptableSettings<T>
    {

        /// <summary>Gets the current <see cref="T"/>.</summary>
        public static T Current => GetSettings();

        static T GetSettings()
        {

            var resource = Resources.Load("Settings/" + typeof(T).Name);
            if (resource)
                return resource is T settings
                    ? settings
                    : null;

            return Create();

        }

        static T Create()
        {

#if UNITY_EDITOR

            EditorFolderUtility.EnsureFolderExists("Assets/Settings/Resources/Settings");

            var obj = CreateInstance<T>();
            AssetDatabase.DeleteAsset("Assets/Settings/Resources/Settings/" + typeof(T).Name + ".asset");
            AssetDatabase.CreateAsset(obj, "Assets/Settings/Resources/Settings/" + typeof(T).Name + ".asset");

            return obj;

#else
        return null;
#endif

        }

        /// <summary>Ensures that the ScriptableSettings has been created.</summary>
        public static void EnsureExists() =>
            GetSettings();

    }

}
