using System;
using System.Collections.Generic;
using System.Linq;
using FistBump.Framework.ExtensionMethods;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

namespace FistBump.Framework.SocialPlatforms
{
    public class LocalPlatform : Singleton<LocalPlatform>, ISocialPlatform
    {
        private static int s_MaximumLeaderboardEntry = 100;

        private LocalUser m_LocalUser;
        private readonly List<UserProfile> m_Friends = new List<UserProfile>();
        private readonly List<UserProfile> m_UserProfiles = new List<UserProfile>();
        private readonly List<Leaderboard> m_Leaderboards = new List<Leaderboard>();
        private readonly List<AchievementDescription> m_AchievementDescriptions = new List<AchievementDescription>();
        private readonly List<Achievement> m_Achievements = new List<Achievement>();
        
        #region Implementation of ISocialPlatform

        #region Users and Friends
        
        public ILocalUser localUser
        {
            get { return m_LocalUser ?? (m_LocalUser = new LocalUser()); }
        }

        public void LoadUsers(string[] userIDs, Action<IUserProfile[]> callback)
        {
            if (!localUser.authenticated)
            {
                callback.SafeInvoke(new IUserProfile[0]);
                return;
            }
            List<UserProfile> list = new List<UserProfile>();
            foreach (string userId in userIDs)
            {
                string userId1 = userId;
                list.AddRange(m_UserProfiles.Where(userProfile => userProfile.id == userId1));
                list.AddRange(m_Friends.Where(userProfile => userProfile.id == userId1));
            }
            callback.SafeInvoke(list.ToArray());
        }

        public void Authenticate(ILocalUser user, Action<bool> callback)
        {
            m_LocalUser = user as LocalUser;
            if (m_LocalUser != null)
            {
                if (!m_LocalUser.authenticated)
                {
                    m_LocalUser.SetAuthenticated(true);
                    m_LocalUser.SetUnderage(false);
                    m_LocalUser.SetUserID("1000");
                    m_LocalUser.SetUserName("Lerp");
                    m_UserProfiles.Add(m_LocalUser);
                    callback.SafeInvoke(true);
                }
                else
                {
                    Debug.Log("[LocalPlatform] Already Authenticated");
                    callback.SafeInvoke(true);
                }
            }
            else
            {
                callback.SafeInvoke(false);
            }
        }

        public void LoadAchievementDescriptions(TextAsset descriptionFile)
        {
            if (descriptionFile != null)
            {
                LoadAchievementDescriptions(descriptionFile.text);
            }
            else
            {
                Debug.Log("[LocalPlatform] No achievement description file provided.");
            }
        }

        private void LoadAchievementDescriptions(string descriptionJSON)
        {
            Hashtable jsonTable = MiniJSON.jsonDecode(descriptionJSON) as Hashtable;

            if (jsonTable != null)
            {
                ArrayList achievementDefinitions = jsonTable["achievementDescriptions"] as ArrayList;
                if (achievementDefinitions != null)
                {
                    foreach (Hashtable achievementDefinition in achievementDefinitions)
                    {
                        string id = achievementDefinition["id"] as string;
                        string title = achievementDefinition["title"] as string;
                        int points = (achievementDefinition["points"] is long ? (int)achievementDefinition["points"] : 0);
                        string achievedDescription = achievementDefinition["achievedDescription"] as string;
                        string unachievedDescription = achievementDefinition["unachievedDescription"] as string;
                        bool hidden = achievementDefinition["hidden"] is bool ? (bool)achievementDefinition["hidden"] : false;

                        m_AchievementDescriptions.Add(new AchievementDescription(id, title, null, achievedDescription, unachievedDescription, hidden, points));
                    }
                }
            }
            else
            {
                if (!MiniJSON.lastDecodeSuccessful())
                {
                    Debug.LogWarning(string.Format("[LocalPlatform] Unable to decode JSON definition file. Error: {0}", MiniJSON.getLastErrorSnippet()));
                }
                else
                {
                    Debug.LogWarning("[LocalPlatform] Unable to decode JSON definition file.");
                }
            }
        }

        public void LoadFriends(ILocalUser user, Action<bool> callback)
        {
            if (!localUser.authenticated)
            {
                callback.SafeInvoke(false);
                return;
            }
            ((LocalUser)user).SetFriends(m_Friends.ToArray());
            callback.SafeInvoke(true);
        }

        #endregion

        #region Achievements

        public void ReportProgress(string achievementID, double progress, Action<bool> callback)
        {
            if (!localUser.authenticated)
            {
                callback.SafeInvoke(false);
                return;
            }
            Achievement achievement = m_Achievements.Find(a => a.id == achievementID);
            if (achievement != null)
            {
                if (achievement.percentCompleted < progress)
                {
                    if (progress >= 100.0)
                    {
                        achievement.SetCompleted(true);
                    }
                    achievement.SetHidden(false);
                    achievement.SetLastReportedDate(DateTime.Now);
                    achievement.percentCompleted = progress;
                    callback.SafeInvoke(true);
                    return;
                }
            }
            AchievementDescription achievementDescription = m_AchievementDescriptions.Find(ad => ad.id == achievementID);
            if (achievementDescription != null)
            {
                m_Achievements.Add(new Achievement(achievementID, progress, (progress >= 100.0), false, DateTime.Now));
                if (callback != null)
                {
                    callback(true);
                }
                return;
            }
            callback.SafeInvoke(false);
        }

        public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
        {
            callback.SafeInvoke(m_AchievementDescriptions.ToArray());
        }

        public void LoadAchievements(Action<IAchievement[]> callback)
        {
            if (!localUser.authenticated)
            {
                callback.SafeInvoke(new Achievement[0]);
                return;
            }
            callback.SafeInvoke(m_Achievements.ToArray());
        }

        public IAchievement CreateAchievement()
        {
            return new Achievement() as IAchievement;
        }

        public void ShowAchievementsUI()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Leaderboards

        public void ReportScore(long score, string board, Action<bool> callback)
        {
            if (!localUser.authenticated)
            {
                callback.SafeInvoke(false);
                return;
            }

            Leaderboard leaderboard = m_Leaderboards.Find(l => l.id == board);
            if (leaderboard != null)
            {
                List<Score> scoreList = new List<Score>((Score[])leaderboard.scores);
                {
                    new Score(board, score, this.localUser.id, DateTime.Now, score + " points", 0);
                };

                scoreList.Sort((Score lhv, Score rhv) => rhv.value.CompareTo(lhv.value));
                for (int i = 0; i < scoreList.Count; i++)
                {
                    scoreList[i].SetRank(i + 1);
                }

                leaderboard.SetScores(scoreList.GetRange(0, Mathf.Min(s_MaximumLeaderboardEntry, scoreList.Count)).ToArray());

                callback.SafeInvoke(true);
                return;
            }

            callback.SafeInvoke(false);

        }

        public void LoadScores(string leaderboardID, Action<IScore[]> callback)
        {
            if (!localUser.authenticated)
            {
                callback.SafeInvoke(new IScore[0]);
                return;
            }
            Leaderboard leaderboard = m_Leaderboards.Find(l => l.id == leaderboardID);
            if (leaderboard != null)
            {
                callback.SafeInvoke(leaderboard.scores);
                return;
            }

            callback.SafeInvoke(new IScore[0]);
        }

        public ILeaderboard CreateLeaderboard()
        {
            return new Leaderboard() as ILeaderboard;
        }

        public void LoadScores(ILeaderboard board, Action<bool> callback)
        {
            if (!localUser.authenticated)
            {
                callback.SafeInvoke(false);
                return;
            }
            Leaderboard leaderboard = (Leaderboard)board;
            foreach (Leaderboard current in m_Leaderboards)
            {
                if (current.id == leaderboard.id)
                {
                    leaderboard.SetTitle(current.title);
                    leaderboard.SetScores(current.scores);
                    leaderboard.SetMaxRange((uint)current.scores.Length);
                }
            }

            List<IScore> scores = new List<IScore>(leaderboard.scores);
            scores.Sort((IScore lhv, IScore rhv) => rhv.value.CompareTo(lhv.value));
            for (int i = 0; i < scores.Count; i++)
            {
                Score score = scores[i] as Score;
                if (score != null)
                {
                    score.SetRank(i + 1);
                }
            }

            IScore myScore = scores.Find(score => score.userID == m_LocalUser.id);
            if (myScore != null)
            {
                leaderboard.SetLocalUserScore(myScore);
            }
            callback.SafeInvoke(true);
        }

        public bool GetLoading(ILeaderboard board)
        {
            return localUser.authenticated && board.loading;
        }

        public void ShowLeaderboardUI()
        {
            throw new NotImplementedException();
        }
        
        #endregion

        #endregion
    }
}