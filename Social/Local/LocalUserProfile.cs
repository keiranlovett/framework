using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

namespace FistBump.Framework.SocialPlatforms
{
    public class LocalUserProfile : IUserProfile
    {
        protected string m_ID;
        protected Texture2D m_Image;
        protected bool m_IsFriend;
        protected UserState m_State;
        protected string m_UserName;

        /*public UserProfile()
        {
            throw new NotImplementedException();
        }
        public UserProfile(string name, string id, bool friend)
        {
            throw new NotImplementedException();
        }
        public UserProfile(string name, string id, bool friend, UserState state, Texture2D image)
        {
            throw new NotImplementedException();
        }*/

        public string id { get; private set; }
        public Texture2D image { get; private set;}
        public bool isFriend { get; private set;}
        public UserState state { get; private set;}
        public string userName { get; private set;}

        public void SetImage(Texture2D image)
        {
            throw new NotImplementedException();
        }
        public void SetIsFriend(bool value)
        {
            throw new NotImplementedException();
        }
        public void SetState(UserState state)
        {
            throw new NotImplementedException();
        }
        public void SetUserID(string id)
        {
            throw new NotImplementedException();
        }
        public void SetUserName(string name)
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}