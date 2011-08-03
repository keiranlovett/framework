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
    #region Enums
    public enum StatisticType
    {
        Min,
        Max,
        Add,
        Replace
    }
    #endregion

    private class FistBumpStatisticDefinition
    {
        private readonly int m_Name = 0;
        private readonly StatisticType m_Type = StatisticType.Add;

        public int Name { get { return m_Name; } }
        public StatisticType Type { get { return m_Type; } }

        public FistBumpStatisticDefinition(int name, StatisticType type)
        {
            m_Name = name;
            m_Type = type;
        }
    }

    #region Private Fields

    private List<FistBumpStatistic> m_Statistics = new List<FistBumpStatistic>();
    private readonly List<FistBumpStatisticDefinition> m_StatisticsDefinitions = new List<FistBumpStatisticDefinition>();

    #endregion

    #region Public Methods

    public void Submit(int statName, int statValue)
    {
        FistBumpStatisticDefinition statDef = m_StatisticsDefinitions.Find(s => s.Name == statName);
        FistBumpStatistic stat = m_Statistics.Find(s => s.Name == statName);
        if (statDef == null)
        {
            Debug.LogWarning(string.Format("[Stats] Stat definition {0} does not exist, you must add the stat before submitting it", statName));
            return;
        }

        bool newStat = stat == null;
        if (newStat)
        {
            stat = new FistBumpStatistic(statName, statDef.Type);
            m_Statistics.Add(stat);
        }
        switch (statDef.Type)
        {
            case StatisticType.Add:
                Debug.Log(string.Format("[Stats] {0} - New Total - Old={1} New={2}", stat.Name, stat.Value, stat.Value + statValue));
                stat.Value += statValue;
                break;
            case StatisticType.Min:
                if (statValue < stat.Value)
                {
                    Debug.Log(string.Format("[Stats] {0} - New Min - {1}New={2}", stat.Name, (!newStat ? string.Format("Old={0} ", stat.Value) : ""), statValue));
                    stat.Value = statValue;
                }
                break;
            case StatisticType.Max:
                if (statValue > stat.Value)
                {
                    Debug.Log(string.Format("[Stats] {0} - New Max - {1}New={2}", stat.Name, (!newStat ? string.Format("Old={0} ", stat.Value) : ""), statValue));
                    stat.Value = statValue;
                }
                break;
            case StatisticType.Replace:
                Debug.Log(string.Format("[Stats] {0} - New Value - {1}New={2}", stat.Name, (!newStat ? string.Format("Old={0} ", stat.Value) : ""), statValue));
                stat.Value = statValue;
                break;
        }
    }

    public void Deserialize(SerializationInfo info)
    {
        m_Statistics = (List<FistBumpStatistic>)info.GetValue("stats", typeof(List<FistBumpStatistic>));
    }

    public void Serialize(SerializationInfo info)
    {
        info.AddValue("stats", m_Statistics);
    }

    public void Add(int statName, StatisticType type)
    {
        if (m_StatisticsDefinitions.Find(s => s.Name == statName) == null)
            m_StatisticsDefinitions.Add(new FistBumpStatisticDefinition(statName, type));
    }

    #endregion

    #region Implementation of MonoBehaviour
    


    #endregion
}

public partial class StatisticName
{

}
