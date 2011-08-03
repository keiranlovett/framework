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
    #region Private Fields

    private readonly int m_Name = 0;
    private int m_CurrentValue = 0;

    #endregion

    #region Public Fields

    public int              Name            { get { return m_Name; } }
    public int              Value           { get { return m_CurrentValue; } set { m_CurrentValue = value; }}

    #endregion

    #region Ctor

    public FistBumpStatistic(int name, FistBumpStatisticManager.StatisticType type)
    {
        m_Name = name;
        if (type == FistBumpStatisticManager.StatisticType.Min)
            m_CurrentValue = int.MaxValue;
    }

    public FistBumpStatistic(SerializationInfo info, StreamingContext ctxt)
    {
        m_Name = (int)info.GetValue("name", typeof(int));
        m_CurrentValue = (int)info.GetValue("currentvalue", typeof(int));
    }

    #endregion

    #region Public Methods

    #endregion

    #region Implementation of ISerializable

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("name", m_Name);
        info.AddValue("currentvalue", m_CurrentValue);
    }

    #endregion
}
