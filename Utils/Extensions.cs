using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FistBump.Framework.ExtensionMethods
{
    public static class Extensions
    {
        #region Transforms Extensions
        public static void SetX(this Transform transform, float x)
        {
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        public static void SetY(this Transform transform, float y)
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }

        public static void SetZ(this Transform transform, float z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }
        #endregion

        #region Safe Invoke Extensions
        public static void SafeInvoke(this Action action)
        {
            if (action != null) action();
        }

        public static void SafeInvoke<T>(this Action<T> action, T t1)
        {
            if (action != null) action(t1);
        }

        public static void SafeInvoke(this EventHandler handler, object sender)
        {
            if (handler != null) handler(sender, EventArgs.Empty);
        }
        #endregion

        #region MonoBehaviour Extensions

        public static T GetInterfaceComponent<T>(this MonoBehaviour obj) where T : class
        {
            return obj.GetComponent(typeof(T)) as T;
        }

        public static T[] FindObjectsOfType<T>(this MonoBehaviour obj) where T : MonoBehaviour
        {
            return Object.FindObjectsOfType(typeof(T)) as T[];
        }

        public static List<T> FindObjectsOfInterface<T>(this MonoBehaviour obj) where T : class
        {
            MonoBehaviour[] monoBehaviours = obj.FindObjectsOfType<MonoBehaviour>();
            List<T> list = new List<T>();

            foreach (MonoBehaviour behaviour in monoBehaviours)
            {
                T component = behaviour.GetComponent(typeof(T)) as T;
                if (component != null)
                {
                    list.Add(component);
                }
            }

            return list;
        }

        public static T GetSafeComponent<T>(this MonoBehaviour obj) where T : MonoBehaviour
        {
            T component = obj.GetComponent<T>();

            if (component == null)
            {
                Debug.LogError(string.Format("Expected to find component of type {0} but found none", typeof(T)), obj);
            }

            return component;
        }
        #endregion
    }
}
