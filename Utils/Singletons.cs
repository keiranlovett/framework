using UnityEngine;
using System.Collections;

namespace FistBump.Framework
{
    /// <summary>
    /// This class is a singleton. Only one instance of this class can exist.
    /// </summary>
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
    {
        private static T s_Instance;

        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        public static T Instance
        {
            get { return s_Instance ?? (s_Instance = (T) FindObjectOfType(typeof (T)) ?? CreateSingletonInstance()); }
        }

        /// <summary>
        /// Create a gameobject with itself attached.
        /// </summary>
        /// <returns>Return an instance of itself.</returns>
        private static T CreateSingletonInstance()
        {
            GameObject gameObject = new GameObject(typeof(T).GetType().Name);
            Debug.LogWarning("Could not find " + gameObject.name + ", creating", gameObject);
            return gameObject.AddComponent<T>();
        }

        /// <summary>
        /// Destroys the singleton. Important for cleaning up the static reference.
        /// </summary>
        public void OnDestroy()
        {
            s_Instance = null;
        }
    }





    /// <summary>
    /// This class is a singleton. Only one instance of this class can exist.
    /// </summary>
    public class Singleton<T> where T : class, new()
    {
        private static T s_Instance;

        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        public static T Instance
        {
            get { return s_Instance ?? (s_Instance = new T()); }
        }

        /// <summary>
        /// Destroys the singleton. Important for cleaning up the static reference.
        /// </summary>
        public void Destroy()
        {
            s_Instance = null;
        }
    }
}