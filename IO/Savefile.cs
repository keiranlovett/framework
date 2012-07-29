#region Using statements

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

#endregion

namespace FistBump.Framework
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>FistBump.ca - Copyright (C)</remarks>
    [Serializable]
    public class Savefile : ISerializable
    {
        #region Private Fields

        private string m_Filename;
        private int m_FormatVersion;

        #endregion

        #region Ctor

        public Savefile()
        {
        }

        public Savefile(string filename, int formatVersion)
        {
            m_Filename = filename;
            m_FormatVersion = formatVersion;
        }

        public Savefile(SerializationInfo info, StreamingContext context)
        {
            //Get the values from info and assign them to the appropriate properties
            m_Filename = (string)info.GetValue("foundGem1", typeof(string));
            m_FormatVersion = (int)info.GetValue("score", typeof(int));
        }

        #endregion

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("foundGem1", (m_Filename));
            info.AddValue("score", m_FormatVersion);
        }

        [OnDeserializing]
        internal void OnDeserializingMethod(StreamingContext context)
        {
            
        }
    }
}