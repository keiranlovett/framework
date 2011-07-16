#region Using statements

using System.Collections.Generic;
using UnityEngine;
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

    #endregion
}
