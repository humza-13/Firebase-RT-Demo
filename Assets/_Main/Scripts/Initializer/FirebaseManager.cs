using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

namespace Phoenix.Firebase.Managers
{
    public class FirebaseManager : MonoBehaviour
    {
        protected static FirebaseAuth Auth;
        protected static FirebaseUser User;
        protected static DependencyStatus DependencyStatus;
        protected static DatabaseReference DatabaseReference;
        protected static FirebaseApp App;
        
        protected virtual void Start()
        {
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