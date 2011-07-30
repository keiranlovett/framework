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

    public void Submit(string name, int value)
    {
        FistBumpStatistic stat = m_Statistics.Find(s => s.Name == name);
        if (stat != null)
            stat.Submit(value);
    }

    public void Serialize(SerializationInfo info)
    {
        m_Statistics = (List<FistBumpStatistic>)info.GetValue("stats", typeof(List<FistBumpStatistic>));
    }

    public void Deserialize(SerializationInfo info)
    {
        info.AddValue("stats", m_Statistics);
    }

    public void Add(string name, FistBumpStatistic.StatisticType type)
    {
        m_Statistics.Add(new FistBumpStatistic(name, type));
    }

    #endregion

    #region Implementation of MonoBehaviour
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        m_Statistics = new List<FistBumpStatistic>();
    }
    #endregion
}
