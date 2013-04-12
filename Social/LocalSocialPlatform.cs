using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FistBump.Framework;
using FistBump.Framework.ExtensionMethods;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

namespace FistBump.Framework
{
    public class LocalPlatformInfo : IPlatformInfo
    {
        public int GetPriority() { return 9999; }//Make sure this is the last plugin to get picked
        public bool IsPlatformSupported()
        {
            return true;
        }
        public void Initialize(TextAsset achievementDescriptions)
        {
            Debug.Log("[LocalPlatform] Connecting Social API to LocalPlatform");
            Social.Active = LocalSocialPlatform.Instance;
            LocalSocialPlatform.Instance.LoadAchievementDescriptions(achievementDescriptions);
        }
    }
    
    [Serializable]
    public class LocalPlatformData
    {
        public LocalUser LocalUser = new LocalUser();
        public List<UserProfile> Friends = new List<UserProfile>();
        public List<UserProfile> UserProfiles = new List<UserProfile>();
        public List<Leaderboard> Leaderboards = new List<Leaderboard>();
        public List<Achievement> Achievements = new List<Achievement>();
    }

    public class LocalSocialPlatform : Singleton<LocalSocialPlatform>, ISocialPlatform
    {
        private static int s_MaximumLeaderboardEntry = 100;

        private LocalPlatformData m_Data = new LocalPlatformData();
        private readonly List<AchievementDescription> m_AchievementDescriptions = new List<AchievementDescription>();
        
        
        #region Implementation of ISocialPlatform

        #region Users and Friends
        
        public ILocalUser localUser
        {
            get { return m_Data.LocalUser ?? (m_Data.LocalUser = new LocalUser()); }
        }

        private void SaveData()
        {

            IO.WriteSecure<LocalPlatformData>(IO.ToPersistentDataPath("localplatform"), m_Data);
        }
        private void LoadData()
        {
            m_Data = IO.ReadSecure<LocalPlatformData>(IO.ToPersistentDataPath("localplatform")) ?? new LocalPlatformData();
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
                list.AddRange(m_Data.UserProfiles.Where(userProfile => userProfile.id == userId1));
                list.AddRange(m_Data.Friends.Where(userProfile => userProfile.id == userId1));
            }
            callback.SafeInvoke(list.ToArray());
        }

        public void Authenticate(ILocalUser user, Action<bool> callback)
        {
            m_Data.LocalUser = user as LocalUser;
            if (m_Data.LocalUser != null)
            {
                if (!m_Data.LocalUser.authenticated)
                {
                    LoadData();
                    m_Data.LocalUser.SetAuthenticated(true);
                    m_Data.LocalUser.SetUnderage(false);
                    m_Data.LocalUser.SetUserID("1000");
                    m_Data.LocalUser.SetUserName("Lerp");
                    if (!m_Data.UserProfiles.Exists(u => u.id == "1000"))
                    {
                        m_Data.UserProfiles.Add(m_Data.LocalUser);
                    }
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
            ((LocalUser)user).SetFriends(m_Data.Friends.ToArray());
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
            Achievement achievement = m_Data.Achievements.Find(a => a.id == achievementID);
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
                    SaveData();
                }
                callback.SafeInvoke(true);
                return;
            }
            AchievementDescription achievementDescription = m_AchievementDescriptions.Find(ad => ad.id == achievementID);
            if (achievementDescription != null)
            {
                m_Data.Achievements.Add(new Achievement(achievementID, progress, (progress >= 100.0), false, DateTime.Now));
                SaveData();
                callback.SafeInvoke(true);
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
            callback.SafeInvoke(m_Data.Achievements.ToArray());
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

            Leaderboard leaderboard = m_Data.Leaderboards.Find(l => l.id == board);
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

                SaveData();
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
            Leaderboard leaderboard = m_Data.Leaderboards.Find(l => l.id == leaderboardID);
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
            foreach (Leaderboard current in m_Data.Leaderboards)
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

            IScore myScore = scores.Find(score => score.userID == m_Data.LocalUser.id);
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