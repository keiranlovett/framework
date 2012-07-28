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
    public class StatisticDefinition
    {
        #region Private Fields
        
        private readonly int m_Name = 0;
	    private readonly string m_Description = "";
	    private readonly StatisticType m_Type = StatisticType.Add;

        #endregion

        #region Public Fields

        public int Name             { get { return m_Name; } }
	    public string Description   { get { return m_Description; } }
	    public StatisticType Type   { get { return m_Type; } }

        #endregion

        #region Ctor

        public StatisticDefinition(int name, string description, StatisticType type)
	    {
	        m_Name = name;
	        m_Description = description;
	        m_Type = type;
        }

        #endregion
    }
}
