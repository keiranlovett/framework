#region Using statements

using UnityEngine;
using System;
using System.Runtime.Serialization;

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

    private readonly int m_Name = 0;
    private readonly StatisticType m_Type = StatisticType.Add;
    private int m_CurrentValue = 0;

    #endregion

    #region Public Fields

    public bool             HasBeenUsed     { get { return (m_Type == StatisticType.Min ? m_CurrentValue != int.MaxValue : m_CurrentValue != 0); } }
    public int              Name            { get { return m_Name; } }
    public StatisticType    Type            { get { return m_Type; } }
    public int              Value           { get { return m_CurrentValue; } }

    #endregion

    #region Ctor

    public FistBumpStatistic(int name, StatisticType type)
    {
        m_Name = name;
        m_Type = type;
        if (type == StatisticType.Min)
            m_CurrentValue = int.MaxValue;
    }

    public FistBumpStatistic(SerializationInfo info, StreamingContext ctxt)
    {
        m_Name = (int)info.GetValue("name", typeof(int));
        m_Type = (StatisticType)info.GetValue("type", typeof(StatisticType));
        m_CurrentValue = (int)info.GetValue("currentvalue", typeof(int));
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
                if (value < m_CurrentValue)
                {
                    Debug.Log(string.Format("[Stats] {0} - New Min - {1}New={2}", m_Name, (HasBeenUsed ? string.Format("Old={0} ", m_CurrentValue) : ""), value));
                    m_CurrentValue = value;
                }
                break;
            case StatisticType.Max:
                if (value > m_CurrentValue)
                {
                    Debug.Log(string.Format("[Stats] {0} - New Max - {1}New={2}", m_Name, (HasBeenUsed ? string.Format("Old={0} ", m_CurrentValue) : ""), value));
                    m_CurrentValue = value;
                }
                break;
            case StatisticType.Replace:
                Debug.Log(string.Format("[Stats] {0} - New Value - {1}New={2}", m_Name, (HasBeenUsed ? string.Format("Old={0} ", m_CurrentValue) : ""), value));
                m_CurrentValue = value;
                break;
        }
    }

    #endregion

    #region Implementation of ISerializable

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("name", m_Name);
        info.AddValue("type", m_Type);
        info.AddValue("currentvalue", m_CurrentValue);
    }

    #endregion
}

public partial class StatisticName
{

}
