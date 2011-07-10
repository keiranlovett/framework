#region Using statements

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#endregion

#region Enums
public enum InputDevices
{
    Keyboard,
    Mouse,
    Gamepad
}
#endregion


/// <summary>
/// 
/// </summary>
/// <remarks>FistBump.ca - Copyright (C)</remarks>
public class FistBumpControlScheme
{
    #region Private Fields

    private readonly InputDevices m_InputDevice;
    private readonly string m_DeviceName;
    private readonly float m_DeadZone;
    #endregion

    #region Public Properties

    public InputDevices InputDevice
    {
        get { return m_InputDevice; }
    }

    public string DeviceName
    {
        get { return m_DeviceName; }
    }

    #endregion

    #region Ctor


    public FistBumpControlScheme(InputDevices inputDevice, string deviceName, float deadZone)
    {
        m_InputDevice = inputDevice;
        m_DeviceName = deviceName;
        m_DeadZone = deadZone;
    }
    #endregion

    #region Public Methods

    public float XAxis(bool ignoreDeadZone = false)
    {
        if (m_InputDevice == InputDevices.Mouse)
            ignoreDeadZone = true;
        float axisValue = Input.GetAxis("Xaxis" + m_DeviceName);
        return ignoreDeadZone ? axisValue : ((axisValue > m_DeadZone || axisValue < -m_DeadZone) ? axisValue : 0.0f);
    }

    public float YAxis(bool ignoreDeadZone = false)
    {
        if (m_InputDevice == InputDevices.Mouse)
            ignoreDeadZone = true;
        float axisValue = Input.GetAxis("Yaxis" + m_DeviceName);
        return ignoreDeadZone ? axisValue : ((axisValue > m_DeadZone || axisValue < -m_DeadZone) ? axisValue : 0.0f);
    }

    public bool GetButtonDown(string actionName)
    {
        return Input.GetButtonDown(actionName + m_DeviceName);
    }

    public bool GetAnyKeyDown(string[] actionNames)
    {
        bool axis = XAxis() != 0 || YAxis() != 0;
        bool button = actionNames.Aggregate(false, (current, actionName) => current || GetButtonDown(actionName));
        return axis || button;
    }

    #endregion

    #region Private Methods

    #endregion
}
