using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Phoenix.Firebase.Managers;
using UnityEngine;

namespace Phoenix.Firebase.Auth
{
    public class AuthManager : FirebaseManager
    {
        public static AuthManager _authInstance;

        protected override void Start()
        {
            base.Start();
            if (dependencyStatus == DependencyStatus.Available) Initialize();
            _authInstance = this;
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

        public async Task CreateOrLoginUser(string email, string password)
        {
            await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(async authTask =>
            {
                if (authTask.IsCanceled)
                    return;

                if (authTask.IsFaulted)
                    await SignIn(email, password);
                
                else
                {
                    AuthResult result = authTask.Result;
                    Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                        result.User.DisplayName, result.User.UserId);
                }
            });

        }
        private async Task SignIn(string email, string password)
        {
           await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(authTask =>
           {
                if (authTask.IsCanceled || authTask.IsFaulted)
                    return;

                AuthResult result = authTask.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
            });
        }

        public FirebaseUser GetCurrentUser()
        {
            return user;
        }
    }
}