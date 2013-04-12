using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FistBump.Framework;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

namespace FistBump.Framework
{
    public interface IPlatformInfo
    {
        int GetPriority();
        bool IsPlatformSupported();
        void Initialize(TextAsset achievementDescriptions);
    }

    /// <summary>
    /// Platform info for the default GameCenter Implementation provided by Unity
    /// </summary>
    public class GameCenterPlatformInfo : IPlatformInfo
    {
        public int GetPriority() { return 1; }
        public bool IsPlatformSupported()
        {
            return Application.platform == RuntimePlatform.IPhonePlayer;
        }
        public void Initialize(TextAsset achievementDescriptions)
        {
            Debug.Log("[SocialPlatformSelector] Leaving Social API connected to GameCenter");
        }
    }

    public static class SocialPlatformSelector
    {
        private static bool s_Selected = false;

        public static TextAsset LocalAchievementDescriptions;

        public static void Select()
        {
            if (s_Selected || Social.localUser.authenticated)
            {
                return;
            }

            var socialPlatforms = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof (IPlatformInfo))
                                                                                  && t.GetConstructor(Type.EmptyTypes) != null).Select(
                                                                                      t => Activator.CreateInstance(t) as IPlatformInfo);

            socialPlatforms.OrderBy(platform => platform.GetPriority());

            foreach (var platform in socialPlatforms.Where(instance => instance.IsPlatformSupported()))
            {
                platform.Initialize(LocalAchievementDescriptions);
                s_Selected = true;
                break;
            }
        }
    }
}