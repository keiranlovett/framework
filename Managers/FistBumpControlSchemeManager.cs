#region Using statements

using System.Collections.Generic;
using UnityEngine;
#endregion



/// <summary>
/// 
/// </summary>
/// <remarks>FistBump.ca - Copyright (C)</remarks>
public class FistBumpControlSchemeManager : MonoBehaviour
{
    #region Private Fields
    private readonly List<FistBumpControlScheme> m_ControlSchemes = new List<FistBumpControlScheme>();
    #endregion

    #region Public Properties
    #endregion

    #region Implementation of MonoBehaviour

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

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

    public void AddControlScheme(InputDevices inputDevice, string deviceName, float deadZone)
    {
        if(m_ControlSchemes.FindAll(s => s.InputDevice == inputDevice && s.DeviceName == deviceName).Count == 0)
        {
            m_ControlSchemes.Add(new FistBumpControlScheme(inputDevice, deviceName, deadZone));
        }
        else
        {
            Debug.Log("Control Scheme already exist."); 
        }
    }

    public FistBumpControlScheme GetControlScheme(string deviceName)
    {
        return m_ControlSchemes.Find(s => s.DeviceName == deviceName);
    }

    #endregion
    
    #region Private Methods

    #endregion
}
