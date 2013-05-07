using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FistBump.Framework.ExtensionMethods
{
    public static class Extensions
    {
        #region String Extensions
        public static string ToCamelCase(this string camelCaseString)
        {
            return System.Text.RegularExpressions.Regex.Replace(camelCaseString, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ").Trim();
        }

        public static string Base64Encode(this string source)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(bytes);
        }

        public static string Base64Decode(this string source)
        {
            var bytes = Convert.FromBase64String(source);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static byte[] GetBytes(this string source)
        {
            return System.Text.Encoding.UTF8.GetBytes(source);
        }

        public static string GetString(this byte[] source)
        {
            return System.Text.Encoding.UTF8.GetString(source);
        }

        public static string ComputeHash(this string value)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(value);
            var hash = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
            var hexDigest = hash.Aggregate("", (x, y) => x + y.ToString("X").ToLower());
            return hexDigest;
        }

        /// <summary>
        /// Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="value">String value to parse</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string value)
        {
            return ToEnum<T>(value, false);
        }

        /// <summary>
        /// Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="value">String value to parse</param>
        /// <param name="ignorecase">Ignore the case of the string being parsed</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string value, bool ignorecase)
        {
            if (value == null)
                throw new ArgumentNullException("Value");

            value = value.Trim();

            if (value.Length == 0)
                throw new ArgumentNullException("Must specify valid information for parsing in the string.", "value");

            Type t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("Type provided must be an Enum.", "T");

            return (T)Enum.Parse(t, value, ignorecase);
        }
        #endregion

        #region LINQ Extensions
        public static bool In<T>(this T source, params T[] list)
        {
            if (null == source) throw new ArgumentNullException("source");
            return list.Contains(source);
        }
        #endregion

        #region Transforms Extensions
        public static void SetPositionX(this Transform transform, float x)
        {
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        public static void SetPositionY(this Transform transform, float y)
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }

        public static void SetPositionZ(this Transform transform, float z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }

        public static void SetRotationX(this Transform transform, float x)
        {
            transform.eulerAngles = new Vector3(x % 360, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        public static void SetRotationY(this Transform transform, float y)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, y % 360, transform.eulerAngles.z);
        }

        public static void SetRotationZ(this Transform transform, float z)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, z % 360);
        }

        public static void SetScaleX(this Transform transform, float x)
        {
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }

        public static void SetScaleY(this Transform transform, float y)
        {
            transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
        }

        public static void SetScaleZ(this Transform transform, float z)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
        }

        public static void SetScaleUniform(this Transform transform, float scale)
        {
            transform.localScale = new Vector3(scale, scale, scale);
        }

        public static void DestroyAllChild(this Transform transform)
        {
            (from Transform child in transform.transform select child.gameObject).ToList().ForEach(child => Object.DestroyImmediate(child));
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
