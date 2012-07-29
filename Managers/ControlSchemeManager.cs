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
    public class ControlSchemeManager
    {
        #region Private Fields
        
        private readonly List<ControlScheme> m_ControlSchemes = new List<ControlScheme>();
        
        #endregion

        #region Public Methods

        public void AddControlScheme(InputDevices inputDevice, string deviceName, float deadZone)
        {
            if (m_ControlSchemes.FindAll(s => s.InputDevice == inputDevice && s.DeviceName == deviceName).Count == 0)
            {
                m_ControlSchemes.Add(new ControlScheme(inputDevice, deviceName, deadZone));
            }
            else
            {
                Debug.Log("Control Scheme already exist.");
            }
        }

        public ControlScheme GetControlScheme(string deviceName)
        {
            return m_ControlSchemes.Find(s => s.DeviceName == deviceName);
        }

        #endregion
    }
}