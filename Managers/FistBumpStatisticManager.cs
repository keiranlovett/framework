#region Using statements

using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

#endregion

/// <summary>
/// 
/// </summary>
/// <remarks>FistBump.ca - Copyright (C)</remarks>
public class FistBumpStatisticManager : MonoBehaviour
{
    #region Private Fields

    private List<FistBumpStatistic> m_Statistics = new List<FistBumpStatistic>();

    #endregion

    #region Public Methods

    public void Submit(int statName, int statValue)
    {
        FistBumpStatistic stat = m_Statistics.Find(s => s.Name == statName);
        if (stat != null)
            stat.Submit(statValue);
    }

    public void Deserialize(SerializationInfo info)
    {
        m_Statistics = (List<FistBumpStatistic>)info.GetValue("stats", typeof(List<FistBumpStatistic>));
    }

    public void Serialize(SerializationInfo info)
    {
        info.AddValue("stats", m_Statistics);
    }

    public void Add(int statName, FistBumpStatistic.StatisticType type)
    {
        if (m_Statistics.Find(s => s.Name == statName) == null)
            m_Statistics.Add(new FistBumpStatistic(statName, type));
    }

    #endregion

    #region Implementation of MonoBehaviour
    


    #endregion
}
