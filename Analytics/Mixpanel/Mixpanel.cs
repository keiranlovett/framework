using System;
using System.Collections.Generic;
using Mixpanel.NET.Engage;
using Mixpanel.NET.Events;
using UnityEngine;

namespace FistBump.Framework
{
    class MixpanelConfig : IAnalyticsConfig
    {
        private readonly string m_ProjectToken;

        public MixpanelConfig(string projectToken)
        {
            m_ProjectToken = projectToken;
        }

        #region Implementation of IAnalyticsConfig

        public IAnalyticsTracker Initialise()
        {
            return new MixpanelAnalyticsTracker(m_ProjectToken);
        }

        #endregion
    }

    class MixpanelAnalyticsTracker : IAnalyticsTracker
    {
        private const string DISTINCT_ID_KEY = "MixpanelDistinctId";
        private readonly MixpanelTracker m_Tracker;
        private readonly MixpanelEngage m_Engage;
        private readonly string m_DistinctID;

        internal MixpanelAnalyticsTracker(string projectToken)
        {
            m_Tracker = new Mixpanel.NET.Events.MixpanelTracker(projectToken, new UnityMixpanelHttp(), new TrackerOptions {Test = true});
            m_Engage = new Mixpanel.NET.Engage.MixpanelEngage(projectToken, new UnityMixpanelHttp(), new EngageOptions());

            if (PlayerPrefs.HasKey(DISTINCT_ID_KEY))
            {
                m_DistinctID = PlayerPrefs.GetString(DISTINCT_ID_KEY);
            }
            else
            {
                m_DistinctID = Guid.NewGuid().ToString("n");
                PlayerPrefs.SetString(DISTINCT_ID_KEY, m_DistinctID);
            }
        }

        #region Implementation of IAnalyticsTracker

        public void Track(string @event, IDictionary<string, object> properties)
        {
            m_Tracker.Track(@event, properties);
        }

        public void Set(IDictionary<string, object> properties)
        {
            m_Engage.Set(m_DistinctID, properties);
        }

        public void Increment(IDictionary<string, object> properties)
        {
            m_Engage.Increment(m_DistinctID, properties);
        }

        #endregion
    }
}