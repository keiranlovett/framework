#region Using statements
using UnityEngine;
#endregion

/// <summary>
/// 
/// </summary>
/// <remarks>FistBump.ca - Copyright (C)</remarks>
public class FistBumpStatistic
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

    private bool m_FirstUse = false;
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
}

public partial class StatisticName
{

}
