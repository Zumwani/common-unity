#pragma warning disable IDE0063 // Use simple 'using' statement

using System;
using System.IO;
using System.Linq;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;

namespace Common
{

    public static class ScriptableSettingsUtility
    {

        public interface IScriptableSettings
        { }

        [InitializeOnLoadMethod]
        public static void EnsureObjectsCreated()
        {

            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.ExportedTypes).Where(t => typeof(IScriptableSettings).IsAssignableFrom(t) && !t.IsAbstract).ToArray();
            foreach (var t in types)
            {
                var method = typeof(ScriptableSettings<>).MakeGenericType(t).GetMethod("EnsureExists", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod);
                method?.Invoke(null, null);
            }

            CreateMenuItems();

        }

        public static void CreateMenuItems()
        {

            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.ExportedTypes).
                Where(t => typeof(IScriptableSettings).IsAssignableFrom(t) && !t.IsAbstract).
                Select(t => (type: t, name: t.Name.Replace("Settings", ""))).
                ToArray();

            EditorFolderUtility.EnsureFolderExists("Assets/Settings");
            var path = "Assets/Settings/MenuItems.cs";

            using (var writer = new StreamWriter(path))
            {

                writer.WriteLine("//Automatically generated class, any changes made will be overwritten.");
                writer.WriteLine("");
                writer.WriteLine("#if UNITY_EDITOR");
                writer.WriteLine("");
                writer.WriteLine("using UnityEditor;");
                writer.WriteLine("");
                writer.WriteLine("namespace Common.Settings.MenuItems");
                writer.WriteLine("{");
                writer.WriteLine("");
                writer.WriteLine("    ///<summary>Automatically generated class to provide menu items for each ScriptableSettings under 'Settings' menu item.");
                writer.WriteLine("    public static class SettingMenuItems");
                writer.WriteLine("    {");

                int i = 0;
                foreach (var (type, name) in types)
                {

                    writer.WriteLine("");
                    writer.WriteLine($@"        [MenuItem(""Settings/{name}"")]");
                    writer.WriteLine($@"        public static void Item{i}() => ScriptableSettingsUtility.OpenInEditor(""Assets/Settings/{type.Name}.asset"");");

                    i += 1;

                }

                writer.WriteLine("");
                writer.WriteLine("    }");
                writer.WriteLine("");
                writer.WriteLine("}");
                writer.WriteLine("#endif");

            }

        }

        public static void OpenInEditor(string path)
        {
            var asset = AssetDatabase.LoadAssetAtPath<ScriptableSettingsProxy>(path);
            if (asset)
                Selection.activeObject = asset;
        }

    }

}
#endif
