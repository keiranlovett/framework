using UnityEngine;
using System.Collections;
using System;

public class FistBumpModel
{
    public event EventHandler Updated;

    #region Oberservable
    protected void NotifyUpdate()
    {
        if (Updated != null)
        {
            Updated(this, EventArgs.Empty);
        }
    }
    #endregion
}
