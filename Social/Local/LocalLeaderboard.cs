using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

namespace FistBump.Framework.SocialPlatforms
{
    public class LocalLeaderboard : ILeaderboard
    {
        #region ILeaderboard Members

        public void SetUserFilter(string[] userIDs)
        {
            throw new NotImplementedException();
        }
        public void LoadScores(Action<bool> callback)
        {
            throw new NotImplementedException();
        }
        public bool loading { get; private set; }
        public string id { get; set; }
        public UserScope userScope { get; set; }
        public Range range { get; set; }
        public TimeScope timeScope { get; set; }
        public IScore localUserScore { get; private set; }
        public uint maxRange { get; private set; }
        public IScore[] scores { get; private set; }
        public string title { get; private set; }

        #endregion

        public override string ToString()
        {
            throw new NotImplementedException();
        }
        public void SetLocalUserScore(IScore score)
        {
            throw new NotImplementedException();
        }
        public void SetMaxRange(uint maxRange)
        {
            throw new NotImplementedException();
        }
        public void SetScores(IScore[] scores)
        {
            throw new NotImplementedException();
        }
        public void SetTitle(string title)
        {
            throw new NotImplementedException();
        }
        public string[] GetUserFilter()
        {
            throw new NotImplementedException();
        }
    }
}