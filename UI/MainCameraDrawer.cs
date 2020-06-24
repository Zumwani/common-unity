using UnityEngine;

namespace Common
{

    #if UNITY_EDITOR

using UnityEditor;

    [CustomPropertyDrawer(typeof(MainCameraAttribute))]
    public class MainCameraDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!property.objectReferenceValue)
                property.objectReferenceValue = Camera.main;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 0;

    }

#endif

    public class MainCameraAttribute : PropertyAttribute
    { }

}