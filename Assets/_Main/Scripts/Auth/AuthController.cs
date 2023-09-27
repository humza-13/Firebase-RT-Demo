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
        #region Actions
        public event Action<FirebaseUser> OnSignedIn;
        public event Action<FirebaseUser> OnSignedOut;
        
        #endregion
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
                   OnSignedOut?.Invoke(User);

                User = Auth.CurrentUser;
                if (isSignedIn)
                   OnSignedIn?.Invoke(User);

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
                    OnSignedIn?.Invoke(User);
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
                OnSignedIn?.Invoke(User);
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