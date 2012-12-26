using System;
using System.ComponentModel;
using System.Reflection;
using FistBump.Framework.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace FistBump.Framework
{
    public class EditorBase<T> : Editor where T : MonoBehaviour
    {
        override public void OnInspectorGUI()
        {
            T data = (T)target;

            GUIContent label = new GUIContent { text = "Properties" };

            DrawDefaultInspectors(label, data);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        public static void DrawDefaultInspectors<R>(GUIContent label, R target) where R : MonoBehaviour
        {
            EditorGUILayout.Separator();
            Type type = typeof(R);
            FieldInfo[] fields = type.GetFields();
            EditorGUI.indentLevel++;

            foreach (FieldInfo field in fields)
            {
                if (field.IsPublic)
                {
                    if (field.FieldType == typeof(int))
                    {
                        field.SetValue(target, EditorGUILayout.IntField(MakeLabel(field), (int)field.GetValue(target)));
                    }
                    else if (field.FieldType == typeof(float))
                    {
                        field.SetValue(target, EditorGUILayout.FloatField(MakeLabel(field), (float)field.GetValue(target)));
                    }

                    ///etc. for other primitive types

                    else if (field.FieldType.IsClass)
                    {
                        var parmTypes = new[] { field.FieldType };

                        const string methodName = "DrawDefaultInspectors";

                        MethodInfo drawMethod = typeof(EditorBase<T>).GetMethod(methodName);

                        if (drawMethod == null)
                        {
                            Debug.LogError("No method found: " + methodName);
                        }
                        else
                        {
                            drawMethod.MakeGenericMethod(parmTypes).Invoke(null, new[] { MakeLabel(field), field.GetValue(target) });
                        }
                    }
                    else
                    {
                        Debug.LogError("DrawDefaultInspectors does not support fields of type " + field.FieldType);
                    }
                }
            }

            EditorGUI.indentLevel--;
        }

        private static GUIContent MakeLabel(FieldInfo field)
        {
            GUIContent guiContent = new GUIContent { text = field.Name.ToCamelCase() };
            object[] descriptions = field.GetCustomAttributes(typeof(DescriptionAttribute), true);

            if (descriptions.Length > 0)
            {
                //just use the first one.
                DescriptionAttribute description = descriptions[0] as DescriptionAttribute;
                if (description != null)
                {
                    guiContent.tooltip = description.Description;
                }
            }

            return guiContent;
        }
    }

}