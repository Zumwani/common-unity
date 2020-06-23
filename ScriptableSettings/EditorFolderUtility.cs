#pragma warning disable IDE0063 // Use simple 'using' statement
#if UNITY_EDITOR

using System.Linq;
using UnityEditor;

namespace Common
{
     
    public static class EditorFolderUtility
    {

        public static void EnsureFolderExists(string folder)
        {

            if (!folder.StartsWith("Assets/"))
                folder = "Assets/" + folder;

            var segments = folder.Split('/');
            var path = segments.FirstOrDefault();
            bool isFirst = true;
            foreach (var f in segments)
            {
                if (f != "Assets" && !AssetDatabase.IsValidFolder(path + "/" + f))
                    AssetDatabase.CreateFolder(path, f);
                if (!isFirst)
                    path += "/" + f;
                isFirst = false;
            }

        }

    }

}
#endif
