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
        private List<IAnalyticsTracker> m_Trackers = new List<IAnalyticsTracker>();
        public void Initialise(IAnalyticsConfig config)
        {
            m_Trackers.Add(config.Initialise());
        }

        public void Track(string @event, IDictionary<string, object> properties)
        {
            m_Trackers.ForEach(t => t.Track(@event, properties));
        }

        public void Set(IDictionary<string, object> properties)
        {
            m_Trackers.ForEach(t => t.Set(properties));
        }

        public void Increment(IDictionary<string, object> properties)
        {
            m_Trackers.ForEach(t => t.Increment(properties));
        }
    }
}