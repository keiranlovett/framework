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

    private List<FistBumpPlayer> m_Players;

    #endregion

    #region Public Properties

    public List<FistBumpPlayer> Players
    {
        get { return m_Players; }
    }

    #endregion

    #region Implementation of MonoBehaviour

    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        m_Players = new List<FistBumpPlayer>();
    }

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Update is called once per frame
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
