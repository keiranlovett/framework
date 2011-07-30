#region Using statements

using UnityEngine;

#endregion

/// <summary>
/// 
/// </summary>
/// <remarks>FistBump.ca - Copyright (C)</remarks>
public class FistBumpPlayer
{
    #region Private Fields

    private readonly int m_ID = -1;
    private readonly string m_Name = "Player";
    private NetworkPlayer m_NetworkPlayer;
    private readonly PlayerMetadata m_Metadata = new PlayerMetadata();
    private FistBumpControlScheme m_ControlScheme = new FistBumpControlScheme(InputDevices.Keyboard, "Arrows", 0.001f);

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

    public FistBumpControlScheme ControlScheme
    {
        get { return m_ControlScheme; }
        set { m_ControlScheme = value; }
    }

    #endregion
    
    #region Ctor

    public FistBumpPlayer(int playerID)
    {
        m_ID = playerID;
        m_Name = "Player " + (playerID+1);
    }

    #endregion

    #region Private Methods

    #endregion
}

public partial class PlayerMetadata
{

}