using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System.Collections.Generic;

namespace FistBump.Framework.SocialPlatforms
{
    public class LocalUser : LocalUserProfile, ILocalUser, IUserProfile
    {
        public LocalUser()
        {
            throw new NotImplementedException();
        }

        public bool authenticated { get; private set; }
        public IUserProfile[] friends { get; private set; }
        public bool underage { get; private set; }

        public void Authenticate(Action<bool> callback)
        {
            throw new NotImplementedException();
        }
        public void LoadFriends(Action<bool> callback)
        {
            throw new NotImplementedException();
        }
        public void SetAuthenticated(bool value)
        {
            throw new NotImplementedException();
        }
        public void SetFriends(IUserProfile[] friends)
        {
            throw new NotImplementedException();
        }
        public void SetUnderage(bool value)
        {
            throw new NotImplementedException();
        }
    }
}