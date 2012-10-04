using System;
using UnityEngine;

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
    }
}
