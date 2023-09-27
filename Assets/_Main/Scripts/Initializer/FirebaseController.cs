using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

namespace Phoenix.Firebase.Managers
{
    public class FirebaseController : MonoBehaviour
    {
        #region Static
        protected static FirebaseAuth Auth;
        protected static FirebaseUser User;
        protected static DependencyStatus DependencyStatus;
        protected static DatabaseReference DatabaseReference;
        protected static FirebaseApp App;
        #endregion
        protected virtual void Start()
        {
            if(DependencyStatus == DependencyStatus.Available) return;
            
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                DependencyStatus = task.Result;
                App  = FirebaseApp.DefaultInstance;
                
                if (DependencyStatus == DependencyStatus.Available)  Debug.Log(
                    "Resolve all Firebase dependencies: " + DependencyStatus);

                else
                    Debug.LogError(
                        "Could not resolve all Firebase dependencies: " + DependencyStatus);
            });
        }
        protected virtual void Initialize() {}
        protected virtual void StateChanged(object sender, System.EventArgs eventArgs) {}
    }
}