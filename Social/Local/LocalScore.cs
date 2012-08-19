using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

namespace FistBump.Framework.SocialPlatforms
{
    public class LocalScore : IScore
    {
        public LocalScore()
        {
            throw new NotImplementedException();
        }
        public LocalScore(string leaderboardID, long value)
        {
            throw new NotImplementedException();
        }
        public LocalScore(string leaderboardID, long value, string userID, DateTime date, string formattedValue, int rank)
        {
            throw new NotImplementedException();
        }

        public DateTime date { get; private set; }
        public string formattedValue { get; private set; }
        public string leaderboardID { get; set; }
        public int rank { get; private set; }
        public string userID { get; private set; }
        public long value { get; set; }

        public void ReportScore(Action<bool> callback)
        {
            throw new NotImplementedException();
        }
        public void SetDate(DateTime date)
        {
            throw new NotImplementedException();
        }
        public void SetFormattedValue(string value)
        {
            throw new NotImplementedException();
        }
        public void SetRank(int rank)
        {
            throw new NotImplementedException();
        }
        public void SetUserID(string userID)
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}