using System;
using FistBump.Framework.SocialPlatforms;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

namespace FistBump.Framework
{
    public static class SocialConnector
    {
        private static bool s_Connected = false;
        //private static bool s_Connecting = false;

        public static TextAsset LocalAchievementDescriptions;

        public static void Connect()
        {
            if (s_Connected || /*s_Connecting ||*/ Social.localUser.authenticated)
            {
                return;
            }

            //s_Connecting = true;

            //Note: For now, on iPhone, keep the GameCenter Implementation and use our disk based implementation for anything else
            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                Debug.Log("[SocialConnector] Connecting Social API to LocalPlatform");
                Social.Active = LocalPlatform.Instance;
                LocalPlatform.Instance.LoadAchievementDescriptions(LocalAchievementDescriptions);
            }
            else
            {
                Debug.Log("[SocialConnector] Leaving Social API connected to GameCenter");
            }
            //TODO: Add support for Kongregate, OpenFeint and other Social Platforms.

            s_Connected = true;
        }
    }
}