#region Using statements

using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using System;
using System.Runtime.Serialization;
using System.Reflection;

#endregion

/// <summary>
/// 
/// </summary>
/// <remarks>FistBump.ca - Copyright (C)</remarks>
[Serializable()]
public class FistBumpStatistic : ISerializable
{
    #region Enums
    public enum StatisticType
    {
        Min,
        Max,
        Add,
        Replace
    }
    #endregion

    #region Private Fields

    private bool m_FirstUse = true;
    private string m_Name = "";
    private StatisticType m_Type = StatisticType.Add;
    private int m_CurrentValue = 0;

    #endregion

    #region Public Fields

    public bool             HasBeenUsed     { get { return !m_FirstUse; } }
    public string           Name            { get { return m_Name; } }
    public StatisticType    Type            { get { return m_Type; } }
    public int              Value           { get { return m_CurrentValue; } }

    #endregion

    #region Ctor
    public FistBumpStatistic(string name, StatisticType type)
    {
        m_Name = name;
        m_Type = type;
    }
    #endregion

    #region Public Methods

    public void Submit(int value)
    {
        switch(m_Type)
        {
            case StatisticType.Add:
                Debug.Log(string.Format("[Stats] {0} - New Total - Old={1} New={2}", m_Name, m_CurrentValue, m_CurrentValue + value));
                m_CurrentValue += value;
                break;
            case StatisticType.Min:
                if (value < m_CurrentValue || m_FirstUse)
                {
                    Debug.Log(string.Format("[Stats] {0} - New Min - {1}New={2}", m_Name, (!m_FirstUse ? string.Format("Old={0} ", m_CurrentValue) : ""), value));
                    m_CurrentValue = value;
                }
                break;
            case StatisticType.Max:
                if (value > m_CurrentValue || m_FirstUse)
                {
                    Debug.Log(string.Format("[Stats] {0} - New Max - {1}New={2}", m_Name, (!m_FirstUse ? string.Format("Old={0} ", m_CurrentValue) : ""), value));
                    m_CurrentValue = value;
                }
                break;
            case StatisticType.Replace:
                Debug.Log(string.Format("[Stats] {0} - New Value - {1}New={2}", m_Name, (!m_FirstUse ? string.Format("Old={0} ", m_CurrentValue) : ""), value));
                m_CurrentValue = value;
                break;
        }

        m_FirstUse = false;
    }

    #endregion

    public FistBumpStatistic(SerializationInfo info, StreamingContext ctxt)
    {
        m_FirstUse = (bool)info.GetValue("firstuse", typeof(bool)); ;
        m_Name = (string)info.GetValue("name", typeof(string)); ;
        m_Type = (StatisticType)info.GetValue("type", typeof(StatisticType)); ;
        m_CurrentValue = (int)info.GetValue("currentvalue", typeof(int));
    }

    //Serialization function.

    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
        info.AddValue("firstuse", m_FirstUse); ;
        info.AddValue("name", m_Name); ;
        info.AddValue("type", m_Type); ;
        info.AddValue("currentvalue", m_CurrentValue);
    }
}

public partial class StatisticName
{

}
