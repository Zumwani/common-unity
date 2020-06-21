#pragma warning disable IDE0063 // Use simple 'using' statement

using System.Linq;

#if UNITY_EDITOR
using UnityEditor;

namespace Common
{
    public static class EditorFolderUtility
    {

        public static void EnsureFolderExists(string folder)
        {

            if (!folder.StartsWith("Assets/"))
                folder = "Assets/";

            var segments = folder.Split('/');
            var path = segments.FirstOrDefault();
            bool isFirst = true;
            foreach (var f in segments)
            {
                if (!AssetDatabase.IsValidFolder(path + "/" + f))
                    AssetDatabase.CreateFolder(path, f);
                if (!isFirst)
                    path += "/" + f;
                isFirst = false;
            }

        }

    }

}
#endif
