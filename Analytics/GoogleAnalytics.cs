using System;
using System.Collections.Generic;

namespace FistBump.Framework
{
    class GoogleAnalyticsConfig : IAnalyticsConfig
    {
        private readonly string m_AccountID;
        private readonly string m_Domain;

        public GoogleAnalyticsConfig(string accountID, string domain)
        {
            m_AccountID = accountID;
            m_Domain = domain;
        }

        #region Implementation of IAnalyticsConfig

        public IAnalyticsTracker Initialise()
        {
            GoogleAnalyticsHelper.Initialize(m_AccountID, m_Domain);
            return new GoogleAnalyticsTracker();
        }

        #endregion
    }

    class GoogleAnalyticsTracker : IAnalyticsTracker
    {
        #region Implementation of IAnalyticsTracker

        public void Track(string @event, IDictionary<string, object> properties)
        {
            GoogleAnalyticsHelper.LogPage(@event);
        }

        public void Set(IDictionary<string, object> properties)
        {
            throw new NotImplementedException();
        }

        public void Increment(IDictionary<string, object> properties)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}