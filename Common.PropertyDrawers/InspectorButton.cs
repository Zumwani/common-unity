using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct InspectorButton
{

    public Action<object> action;

    public static implicit operator InspectorButton(Action action) => Create(action);

    public static InspectorButton Create(Action action) =>
        new InspectorButton() { action = (caller) => action?.Invoke() };

    public static InspectorButton Create<Parameter>(Parameter parameter, Action<Parameter> action) =>
        new InspectorButton() { action = (caller) => action?.Invoke(parameter) };

    public static InspectorButton Create<This>(Action<This> action) =>
        new InspectorButton() { action = (caller) => action?.Invoke(GetThis<This>(caller)) };

    public static InspectorButton Create<This, Parameter>(Parameter parameter, Action<This, Parameter> action) =>
        new InspectorButton() { action = (caller) => action?.Invoke(GetThis<This>(caller), parameter) };

    static T GetThis<T>(object caller) =>
        typeof(T).IsAssignableFrom(caller?.GetType())
        ? (T)caller
        : default;

    public void Invoke(object caller)
    {
        if (action != null)
            action.Invoke(caller);
        else
        {
            Debug.Log(caller);

        }
    }

}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InspectorButton))]
public class ButtonDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (GUI.Button(position, label))
        {
            var action = (InspectorButton)property.GetValue();
            action.Invoke(property.serializedObject.targetObject); 
        }
    }

}
#endif
