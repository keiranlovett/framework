#region Using statements

using UnityEngine;

#endregion

namespace FistBump.Framework
{
    public abstract class GameService : MonoBehaviour
    {
        private bool m_APILoaded = false;
        public bool APILoaded
        {
            get { return m_APILoaded; }
            set { m_APILoaded = value; }
        }

        public abstract void Initialize();
    }
}
