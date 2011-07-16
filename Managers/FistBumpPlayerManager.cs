#region Using statements

using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// 
/// </summary>
/// <remarks>FistBump.ca - Copyright (C)</remarks>
public class FistBumpPlayerManager : MonoBehaviour
{
    #region Private Fields

    private List<FistBumpPlayer> m_Players = new List<FistBumpPlayer>();

    #endregion

    #region Public Properties

    public List<FistBumpPlayer> Players
    {
        get { return m_Players; }
    }

    #endregion

    #region Public Methods

    public FistBumpPlayer AddPlayer()
    {
        m_Players.Add(new FistBumpPlayer(m_Players.Count));
        return m_Players[m_Players.Count-1];
    }
    #endregion

    #region Private Methods

    #endregion
}
