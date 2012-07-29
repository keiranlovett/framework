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
    public class Task : Statistic
    {
        #region Constants

        private const String KEY_GOAL = "g";

        #endregion

        #region Private Fields

        private int m_Goal = 0;

        #endregion

        #region Public Fields

        public int Goal { get { return m_Goal; } set { m_Goal = value; } }

        #endregion

        #region Ctor

        public Task(int name, StatisticType type, int goal) : base(name, type)
        {
            m_Goal = goal;
        }

        public Task(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            m_Goal = (int)info.GetValue(KEY_GOAL, typeof(int));
        }

        #endregion

        #region Public Methods

        #endregion

        #region Implementation of ISerializable

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(KEY_GOAL, m_Goal);
        }

        #endregion
    }
}