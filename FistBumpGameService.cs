#region Using statements

using UnityEngine;

#endregion

public abstract class FistBumpGameService : MonoBehaviour
{
    private bool m_APILoaded = false;
    public bool APILoaded
    {
        get { return m_APILoaded; }
        set { m_APILoaded = value; }
    }

    public abstract void Initialize();
}

