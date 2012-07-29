#region Using statements

using UnityEngine;
using System;
using System.Runtime.Serialization;

#endregion

namespace FistBump.Framework
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>FistBump.ca - Copyright (C)</remarks>
    [Serializable]
    public class Statistic : ISerializable
    {
        #region Constants

        private const String KEY_NAME = "id";
        private const String KEY_VALUE = "v";

        #endregion

        #region Private Fields

        private readonly int m_Name = 0;
        private int m_CurrentValue = 0;
        private bool m_IsDirty = false;

        #endregion

        #region Public Fields

        public int Name { get { return m_Name; } }
        public bool IsDirty { get { return m_IsDirty; } }
        public int Value
        {
            get { return m_CurrentValue; }
            set
            {
                m_CurrentValue = value;
                m_IsDirty = true;
            } 
        }

        #endregion

        #region Ctor

        public Statistic(int name, StatisticType type)
        {
            m_Name = name;
            if (type == StatisticType.Min)
                m_CurrentValue = int.MaxValue;
        }

        public Statistic(SerializationInfo info, StreamingContext context)
        {
            m_Name = (int)info.GetValue(KEY_NAME, typeof(int));
            m_CurrentValue = (int)info.GetValue(KEY_VALUE, typeof(int));
        }

        #endregion

        #region Public Methods

        #endregion

        #region Implementation of ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(KEY_NAME, m_Name);
            info.AddValue(KEY_VALUE, m_CurrentValue);
        }

        #endregion
    }
}