using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

namespace Phoenix.Firebase.Managers
{
    public class FirebaseManager : MonoBehaviour
    {
        protected static FirebaseAuth auth;
        protected static FirebaseUser user;
        protected static DependencyStatus dependencyStatus;
        
        protected virtual void Start()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)  Debug.Log(
                    "Resolve all Firebase dependencies: " + dependencyStatus);

                else
                    Debug.LogError(
                        "Could not resolve all Firebase dependencies: " + dependencyStatus);
            });
        }
        protected virtual void Initialize()
        {
            
        }

        protected virtual void StateChanged(object sender, System.EventArgs eventArgs)
        {
           
        }
    }
}