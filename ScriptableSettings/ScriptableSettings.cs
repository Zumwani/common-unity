using UnityEngine;
using System.Linq;

namespace Common
{

#if UNITY_EDITOR
    using UnityEditor;
#endif

    public abstract class ScriptableSettings<T> : ScriptableObject, ScriptableSettingsUtility.IScriptableSettings where T : ScriptableObject
    {

        public static T Current => GetSettings();

        static T GetSettings()
        {

            var resource = Resources.Load("Settings/" + typeof(T).Name);
            if (resource)
                return resource is T settings
                    ? settings
                    : null;

#if UNITY_EDITOR

            EditorFolderUtility.EnsureFolderExists("Assets/Settings/Resources/Settings");

            var obj = CreateInstance<T>();
            AssetDatabase.DeleteAsset("Assets/Settings/Resources/Settings/" + typeof(T).Name + ".asset");
            AssetDatabase.CreateAsset(obj, "Assets/Settings/Resources/Settings/" + typeof(T).Name + ".asset");

            var proxy = CreateInstance<ScriptableSettingsProxy>();
            proxy.target = obj;

            AssetDatabase.DeleteAsset("Assets/Settings/" + typeof(T).Name + ".asset");
            AssetDatabase.CreateAsset(proxy, "Assets/Settings/" + typeof(T).Name + ".asset");

            return obj;

#else
        return null;
#endif

        }

        public static void EnsureExists()
        {
            GetSettings();
        }

        public static void OpenInEditor()
        {
#if UNITY_EDITOR
            EnsureExists();
            var asset = AssetDatabase.LoadAssetAtPath<ScriptableSettingsProxy>("Assets/Settings/" + typeof(T).Name + ".asset");
            if (asset)
                Selection.activeObject = asset;
#endif
        }

    }

}
