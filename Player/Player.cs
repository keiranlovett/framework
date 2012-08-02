#region Using statements

using UnityEngine;

#endregion

namespace FistBump.Framework
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>FistBump.ca - Copyright (C)</remarks>
    public class Player
    {
        #region Private Fields

        private readonly int m_ID = -1;
        private readonly string m_Name = "Player";
        private NetworkPlayer m_NetworkPlayer;
        private readonly PlayerMetadata m_Metadata = new PlayerMetadata();
        private ControlScheme m_ControlScheme = new ControlScheme(InputDevices.Keyboard, "Arrows", 0.001f);

        #endregion

        #region Public Properties

        public int ID
        {
            get { return m_ID; }
        }
        public string Name
        {
            get { return m_Name; }
        }
        public NetworkPlayer NetworkPlayer
        {
            get { return m_NetworkPlayer; }
            set { m_NetworkPlayer = value; }
        }
        public PlayerMetadata Metadata
        {
            get { return m_Metadata; }
        }

        public ControlScheme ControlScheme
        {
            get { return m_ControlScheme; }
            set { m_ControlScheme = value; }
        }

        #endregion

        #region Ctor

        public Player(int playerID)
        {
            m_ID = playerID;
            m_Name = "Player " + (playerID + 1);
        }

        #endregion

        #region Private Methods

        #endregion
    }

    public partial class PlayerMetadata
    {

    }
}