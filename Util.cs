using UnityEngine;
using System.Collections;

namespace FistBump.Framework
{
    public static class Util
    {
        public static void IgnoreCollision(GameObject obj, string tag)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject o in objects)
            {
                if (o.GetComponent("Collider") && o != obj)
                    Physics.IgnoreCollision(obj.collider, o.collider);
            }
        }

        public static void PauseGame()
        {
            Object[] objects = Object.FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in objects)
            {
                go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
            }
        }

        public static void ResumeGame()
        {
            Object[] objects = Object.FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in objects)
            {
                go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}