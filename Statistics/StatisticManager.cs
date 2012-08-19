#region Using statements

using System;
using System.Collections;
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

        static StatisticManager() { }
        static readonly StatisticManager s_Instance = new StatisticManager();
        public static StatisticManager Instance { get { return s_Instance; } }

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

        public void LoadDefinitions(TextAsset definitionFile)
        {
            if (definitionFile != null)
            {
                LoadDefinitions(definitionFile.text);
            }
        }
        public void LoadDefinitions(string definitionJSON)
        {
            Hashtable jsonTable = MiniJSON.jsonDecode(definitionJSON) as Hashtable;

            if (jsonTable != null)
            {
                ArrayList statDefinitions = jsonTable["statDefinitions"] as ArrayList;
                if (statDefinitions != null)
                {
                    foreach(Hashtable statDefinition in statDefinitions)
                    {
                        string id = statDefinition["id"] as string;
                        string desc = statDefinition["description"] as string;
                        string typeString = statDefinition["type"] as string;

                        if (typeString != null)
                        {

                            try
                            {
                                StatisticType statisticType = (StatisticType)Enum.Parse(typeof(StatisticType), typeString, true);
                                if (Enum.IsDefined(typeof(StatisticType), statisticType))
                                {
                                    if (id == null)
                                    {
                                        Debug.LogWarning("[Stats] Missing id");
                                        continue;
                                    }
                                    if (desc == null)
                                    {
                                        Debug.LogWarning("[Stats] Missing description");
                                        continue;
                                    }

                                    AddDefinition(id, desc, statisticType);
                                }
                                else
                                {
                                    Debug.LogWarning(string.Format("[Stats] {0} is not an underlying value of the StatisticType enumeration.", typeString));
                                }

                            }
                            catch (ArgumentException)
                            {
                                Debug.LogWarning(string.Format("[Stats] {0} is not a member of the StatisticType enumeration.", typeString));
                            }
                        }
                    }
                }
            }
            else
            {
                if (!MiniJSON.lastDecodeSuccessful())
                {
                    Debug.LogWarning(string.Format("[Stats] Unable to decode JSON definition file. Error: {0}", MiniJSON.getLastErrorSnippet()));
                }
                else
                {
                    Debug.LogWarning("[Stats] Unable to decode JSON definition file.");
                }
            }
        }

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
	}
}
