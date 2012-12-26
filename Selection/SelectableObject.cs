using UnityEngine;

namespace FistBump.Framework
{
    public class SelectableObject : MonoBehaviour
    {
        public void OnSelected()
        {
            Debug.Log(string.Format("{0} Selected", gameObject.name));
        }

        public void OnDeselected()
        {
            Debug.Log(string.Format("{0} Deselected", gameObject.name));
        }
    }
}