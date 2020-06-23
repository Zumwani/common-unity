#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class PropertyDrawerExtensions
{

    public static object GetParent(this SerializedProperty prop)
    {

        var path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        var elements = path.Split('.');

        foreach (var element in elements.Take(elements.Length - 1))
            if (element.Contains("["))
            {
                var elementName = element.Substring(0, element.IndexOf("["));
                var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = SerializedPropertyUtility.GetValue(obj, elementName, index);
            }
            else
                obj = SerializedPropertyUtility.GetValue(obj, element);

        return obj;

    }

    public static GUIStyle Color(this GUIStyle style, Color color)
    {
        style = new GUIStyle(style);
        style.normal.textColor = color;
        return style;
    }

    public static void RepaintInspector(this SerializedObject BaseObject)
    {
        var inspector = ActiveEditorTracker.sharedTracker.activeEditors.FirstOrDefault(i => i.serializedObject == BaseObject);
        inspector.Repaint();
    }

    public static bool Is(this Type type, Type baseType)
    {

        if (type == null) return false;
        if (baseType == null) return false;

        return baseType.IsAssignableFrom(type);

    }

    public static bool Is<T>(this Type type)
    {

        if (type == null) return false;
        Type baseType = typeof(T);

        return baseType.IsAssignableFrom(type);
    }

}
#endif
