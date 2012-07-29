#region Using statements

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace FistBump.Framework
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>FistBump.ca - Copyright (C)</remarks>
    public class PlayerManager
    {
        #region Private Fields

        private readonly List<Player> m_Players = new List<Player>();

        #endregion

        #region Public Properties

        public List<Player> Players
        {
            get { return m_Players; }
        }

        #endregion

        #region Public Methods

        public Player AddPlayer()
        {
            m_Players.Add(new Player(m_Players.Count));
            return m_Players[m_Players.Count - 1];
        }
        #endregion

        #region Private Methods

        #endregion
    }
}