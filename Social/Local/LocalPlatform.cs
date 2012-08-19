using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

namespace FistBump.Framework.SocialPlatforms
{
    public class LocalPlatform : ISocialPlatform
    {
        #region Implementation of ISocialPlatform

        private LocalUser m_LocalUser;
        private List<LocalAchievementDescription> m_AchievementDescriptions;

        public ILocalUser localUser
        {
            get { return m_LocalUser; }
        }

        public void LoadUsers(string[] userIDs, Action<IUserProfile[]> callback)
        {
            throw new NotImplementedException();
        }

        public void ReportProgress(string achievementID, double progress, Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
        {
            throw new NotImplementedException();
        }

        public void LoadAchievements(Action<IAchievement[]> callback)
        {
            throw new NotImplementedException();
        }

        public IAchievement CreateAchievement()
        {
            throw new NotImplementedException();
        }

        public void ReportScore(long score, string board, Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public void LoadScores(string leaderboardID, Action<IScore[]> callback)
        {
            throw new NotImplementedException();
        }

        public ILeaderboard CreateLeaderboard()
        {
            throw new NotImplementedException();
        }

        public void ShowAchievementsUI()
        {
            throw new NotImplementedException();
        }

        public void ShowLeaderboardUI()
        {
            throw new NotImplementedException();
        }

        public void Authenticate(ILocalUser user, Action<bool> callback)
        {
            m_LocalUser = user as LocalUser;
            if (m_LocalUser != null)
            {
                if (!m_LocalUser.authenticated)
                {
                    m_LocalUser.Authenticate(callback);
                }
                else
                {
                    Debug.Log("Already Authenticated");
                    callback(true);
                }
            }
            else
            {
                callback(false);
            }
        }

        public void LoadFriends(ILocalUser user, Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public void LoadScores(ILeaderboard board, Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public bool GetLoading(ILeaderboard board)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}