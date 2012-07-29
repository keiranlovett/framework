#region Using statements

using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

#endregion

namespace FistBump.Framework
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>FistBump.ca - Copyright (C)</remarks>
	public class StatisticManager
    {
        #region Singleton

        static readonly StatisticManager s_Instance = new StatisticManager();
        static StatisticManager()
        {
        }

        public static StatisticManager Instance
        {
            get
            {
                return s_Instance;
            }
        }
        #endregion

        #region Private Fields

        private List<Statistic> m_Statistics = new List<Statistic>();
	    private readonly List<StatisticDefinition> m_StatisticsDefinitions = new List<StatisticDefinition>();
        private static bool s_IsVerbose = true;
	
	    #endregion

        #region Public Accessor

        public static bool IsVerbose { get { return s_IsVerbose; } set { s_IsVerbose = value; } }

        #endregion
	
	    #region Public Methods

        public void Submit(string statName, int statValue)
        {
            Submit((int) CRC32.Calculate(statName), statValue);
        }
	
	    public void Submit(int statName, int statValue)
	    {
	        StatisticDefinition statDef = m_StatisticsDefinitions.Find(s => s.Name == statName);
	        Statistic stat = m_Statistics.Find(s => s.Name == statName);
	        if (statDef == null)
	        {
	            Debug.LogWarning(string.Format("[Stats] Stat definition {0} does not exist, you must add the stat before submitting it", statName));
	            return;
	        }
	
	        bool newStat = stat == null;
	        if (newStat)
	        {
	            stat = new Statistic(statName, statDef.Type);
	            m_Statistics.Add(stat);
	        }
	        switch (statDef.Type)
	        {
	            case StatisticType.Add:
                    if(IsVerbose)
	                    Debug.Log(string.Format("[Stats] {0} - New Total - Old={1} New={2}", statDef.Description, stat.Value, stat.Value + statValue));
	                stat.Value += statValue;
	                break;
	            case StatisticType.Min:
	                if (statValue < stat.Value)
	                {
                        if (IsVerbose)
                            Debug.Log(string.Format("[Stats] {0} - New Min - {1}New={2}", statDef.Description, (!newStat ? string.Format("Old={0} ", stat.Value) : ""), statValue));
	                    stat.Value = statValue;
	                }
	                break;
	            case StatisticType.Max:
	                if (statValue > stat.Value)
	                {
                        if (IsVerbose)
                            Debug.Log(string.Format("[Stats] {0} - New Max - {1}New={2}", statDef.Description, (!newStat ? string.Format("Old={0} ", stat.Value) : ""), statValue));
	                    stat.Value = statValue;
	                }
	                break;
	            case StatisticType.Replace:
                    if (IsVerbose)
                        Debug.Log(string.Format("[Stats] {0} - New Value - {1}New={2}", statDef.Description, (!newStat ? string.Format("Old={0} ", stat.Value) : ""), statValue));
	                stat.Value = statValue;
	                break;
	        }
	    }
	
	    public void Deserialize(SerializationInfo info)
	    {
	        m_Statistics = (List<Statistic>)info.GetValue("stats", typeof(List<Statistic>));
	    }
	
	    public void Serialize(SerializationInfo info)
	    {
	        info.AddValue("stats", m_Statistics);
	    }

        public void AddDefinition(string statName, string description, StatisticType type)
        {
            AddDefinition((int) CRC32.Calculate(statName), description, type);
        }
	
	    public void AddDefinition(int statName, string description, StatisticType type)
	    {
	        if (m_StatisticsDefinitions.Find(s => s.Name == statName) == null)
	            m_StatisticsDefinitions.Add(new StatisticDefinition(statName, description, type));
	    }
	
	    #endregion
	
	    #region Implementation of MonoBehaviour
	    
	
	
	    #endregion
	}
}
