#if UNITY_EDITOR

using System.Collections;
using System.Reflection;
using UnityEditor;

namespace Common
{

    public static class SerializedPropertyUtility
    {

        /// <summary>Gets the value of this <see cref="SerializedProperty"/>. Taken from 'https://github.com/lordofduct/spacepuppy-unity-framework'.</summary>
        public static object GetValue(this SerializedProperty prop)
        {

            if (prop == null) 
                return null;

            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');

            foreach (var element in elements)
            {

                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue(obj, elementName, index);
                }
                else
                    obj = GetValue(obj, element);

            }

            return obj;

        }

        public static object GetValue(object source, string name)
        {

            if (source == null)
                return null;

            var type = source.GetType();

            while (type != null)
            {

                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(source);

                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);

                type = type.BaseType;

            }

            return null;

        }

        public static object GetValue(object source, string name, int index)
        {

            if (!(GetValue(source, name) is IEnumerable enumerable))
                return null;

            var enm = enumerable.GetEnumerator();
            for (int i = 0; i <= index; i++)
                if (!enm.MoveNext()) return null;

            return enm.Current;

        }

    }

}
#endif
