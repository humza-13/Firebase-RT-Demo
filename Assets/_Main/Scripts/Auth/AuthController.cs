using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Phoenix.Firebase.Managers;
using UnityEngine;

namespace Phoenix.Firebase.Auth
{
    public class AuthController : FirebaseController
    {
        #region Singleton
        private static AuthController _authInstance;

        public static AuthController AuthInstance
        {
            get { return _authInstance; }
            private set { _authInstance = value; }
        }
        private void Awake()
        {
            if (_authInstance != null && _authInstance != this)
            {
                Destroy(gameObject);
                return;
            }

            AuthInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        #region Overrides
        protected override void Start()
        {
            base.Start();
            if (DependencyStatus == DependencyStatus.Available) Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
            Auth = FirebaseAuth.DefaultInstance;
            Auth.StateChanged += StateChanged;
            StateChanged(this, null);
        }

        protected override void StateChanged(object sender, EventArgs eventArgs)
        {
            base.StateChanged(sender, eventArgs);
            if (Auth.CurrentUser != User)
            {
                bool isSignedIn = User != Auth.CurrentUser && Auth.CurrentUser != null
                                                           && Auth.CurrentUser.IsValid();
                if (!isSignedIn && User != null)
                    Debug.Log("Signed out " + User.UserId);

                User = Auth.CurrentUser;
                if (isSignedIn)
                    Debug.Log("Signed in " + User.UserId);

            }
        }
        #endregion

        #region Auth
        public async Task CreateOrLoginUser(string email, string password)
        {
            await Auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(async authTask  =>
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
           await Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(authTask =>
           {
                if (authTask.IsCanceled || authTask.IsFaulted)
                    return;

                AuthResult result = authTask.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
            });
        }
        #endregion

        #region Helper
        public FirebaseUser GetCurrentUser()
        {
            return User;
        }
        #endregion
    }
}