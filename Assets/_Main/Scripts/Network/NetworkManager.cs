using Firebase.Auth;
using Phoenix.Firebase.Auth;
using Phoenix.Firebase.RT;
using UnityEngine;

namespace Phoenix.Network
{
    [RequireComponent(typeof(AuthController))]
    [RequireComponent(typeof(RTController))]
    public class NetworkManager : MonoBehaviour
    {
        private void Awake()
        {
            AuthController.AuthInstance.OnSignedIn += OnUserSignedIn;
            AuthController.AuthInstance.OnSignedOut += OnUserSignedOut;
            
            RTController.RTInstance.OnPlayerJoin += OnPlayerJoin;
            RTController.RTInstance.OnPlayerLeft += OnPlayerLeft;
            RTController.RTInstance.OnGameStart += OnGameStartEvent;
        }
        
        #region Match Making
        protected virtual void HostGame() => 
            RTController.RTInstance.HostGame(AuthController.AuthInstance.GetCurrentUser().UserId);
        protected virtual void JoinGame() => 
            RTController.RTInstance.JoinGame(AuthController.AuthInstance.GetCurrentUser().UserId);
        #endregion

        #region Events
        protected virtual void OnGameStartEvent() =>
            Debug.Log("Game Started");
        
        protected virtual void OnPlayerJoin(string uuid) =>
            Debug.Log("Player: " + uuid + " joined");
        
        protected virtual void OnPlayerLeft(string uuid) =>
            Debug.Log("Player: " + uuid + " left");
        protected virtual void OnUserSignedIn(FirebaseUser user) => 
             Debug.Log(user.UserId + " is logged in");
        protected virtual void OnUserSignedOut(FirebaseUser user) => 
            Debug.Log(user.UserId + " is logged out");

        #endregion

    }
}