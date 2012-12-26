#region Using statements

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
#endregion

namespace FistBump.Framework
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>FistBump.ca - Copyright (C)</remarks>
    public static class Log
    {
        static List<KeyValuePair<float, int>> values = new List<KeyValuePair<float, int>>();

        public static void Variable(string name, float x, int y)
        {
            values.Add(new KeyValuePair<float, int>(x, y));
        }

        public static void Dump()
        {
            string filePath = @"test.csv";


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("date,close");
            foreach (KeyValuePair<float, int> v in values)
            {
                sb.AppendLine(string.Format("{0},{1}", v.Key, v.Value));
            }

            File.WriteAllText(filePath, sb.ToString());
        }
    }

}