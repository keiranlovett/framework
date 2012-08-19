#region Using statements

using System.Collections.Generic;
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
    public class StatisticsToAchievement
    {
        #region Constants

        private const String KEY_TASKS = "tasks";

        #endregion

        #region Private Fields

        private readonly List<Task> m_Tasks = new List<Task>();

        #endregion

        #region Ctor

        public StatisticsToAchievement()
        {
            
        }

        public StatisticsToAchievement(SerializationInfo info, StreamingContext context)
        {
            m_Tasks = (List<Task>)info.GetValue(KEY_TASKS, typeof(List<Task>));
        }

        #endregion

        #region Implementation of ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(KEY_TASKS, m_Tasks);
        }

        #endregion
    }
}