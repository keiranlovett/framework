using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

namespace FistBump.Framework.SocialPlatforms
{
    [System.Serializable]
    public class LocalAchievementDescription : IAchievementDescription
    {
        #region IAchievementDescription Members

        public string achievedDescription { get; private set; }
        public bool hidden { get; private set; }
        public string id { get; set; }
        public Texture2D image { get; private set; }
        public int points { get; private set; }
        public string title { get; private set; }
        public string unachievedDescription { get; private set; }

        #endregion
    }
}