using System;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

namespace Phoenix.Firebase.Managers
{
    public class AuthManager : FirebaseManager
    {
        protected override void Start()
        {
            base.Start();
            if(dependencyStatus == DependencyStatus.Available) Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
            auth = FirebaseAuth.DefaultInstance;
            auth.StateChanged += StateChanged;
            StateChanged(this, null);
        }

        protected override void StateChanged(object sender, EventArgs eventArgs)
        {
            base.StateChanged(sender, eventArgs);
            if (auth.CurrentUser != user)
            {
                bool isSignedIn = user != auth.CurrentUser && auth.CurrentUser != null
                                                         && auth.CurrentUser.IsValid();
                if (!isSignedIn && user != null)
                    Debug.Log("Signed out " + user.UserId);

                user = auth.CurrentUser;
                if (isSignedIn)
                    Debug.Log("Signed in " + user.UserId);
                
            }
        }
    }
}