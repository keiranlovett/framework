using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

