using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

namespace FistBump.Framework.SocialPlatforms
{
    public class LocalAchievement : IAchievement
    {
        public LocalAchievement(string id, double percentCompleted, bool completed, bool hidden, DateTime lastReportedDate)
        {
            this.id = id;
            this.percentCompleted = percentCompleted;
            this.completed = completed;
            this.hidden = hidden;
            this.lastReportedDate = lastReportedDate;
        }
        public LocalAchievement(string id, double percent)
        {
            throw new NotImplementedException();
        }

        public LocalAchievement()
        {
            throw new NotImplementedException();
        }

        #region IAchievement Members

        public void ReportProgress(Action<bool> callback)
        {
            throw new NotImplementedException();
        }
        public string id { get; set; }
        public double percentCompleted { get; set; }
        public bool completed { get; private set; }
        public bool hidden { get; private set; }
        public DateTime lastReportedDate { get; private set; }

        #endregion

        public override string ToString()
        {
            throw new NotImplementedException();
        }
        public void SetCompleted(bool value)
        {
            throw new NotImplementedException();
        }
        public void SetHidden(bool value)
        {
            throw new NotImplementedException();
        }
        public void SetLastReportedDate(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}