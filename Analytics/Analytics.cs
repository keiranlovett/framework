using System.Collections.Generic;

namespace FistBump.Framework
{

    interface IAnalyticsTracker
    {
        void Track(string @event, IDictionary<string, object> properties);
        void Set(IDictionary<string, object> properties);
        void Increment(IDictionary<string, object> properties);
    }

    interface IAnalyticsConfig
    {
        IAnalyticsTracker Initialise();
    }

    class Analytics : Singleton<Analytics>
    {
        private IAnalyticsTracker m_Tracker;
        public void Initialise(IAnalyticsConfig config)
        {
            m_Tracker = config.Initialise();
        }

        public void Track(string @event, IDictionary<string, object> properties)
        {
            m_Tracker.Track(@event, properties);
        }

        public void Set(IDictionary<string, object> properties)
        {
            m_Tracker.Set(properties);
        }

        public void Increment(IDictionary<string, object> properties)
        {
            m_Tracker.Increment(properties);
        }
    }
}