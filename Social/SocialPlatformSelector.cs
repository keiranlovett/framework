using System;
using FistBump.Framework;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

namespace FistBump.Framework
{
    public static class SocialPlatformSelector
    {
        private static bool s_Selected = false;
        //private static bool s_Connecting = false;

        public static TextAsset LocalAchievementDescriptions;

        public static void Select()
        {
            if (s_Selected || /*s_Connecting ||*/ Social.localUser.authenticated)
            {
                return;
            }

            //s_Connecting = true;

            //Note: For now, on iPhone, keep the GameCenter Implementation and use our disk based implementation for anything else
            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                Debug.Log("[SocialConnector] Connecting Social API to LocalPlatform");
                Social.Active = LocalSocialPlatform.Instance;
                LocalSocialPlatform.Instance.LoadAchievementDescriptions(LocalAchievementDescriptions);
            }
            else
            {
                Debug.Log("[SocialConnector] Leaving Social API connected to GameCenter");
            }
            //TODO: Add support for Kongregate, OpenFeint and other Social Platforms.

            s_Selected = true;
        }
    }
}