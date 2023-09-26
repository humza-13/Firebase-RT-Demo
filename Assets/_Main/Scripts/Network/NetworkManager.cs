using System;
using Firebase.Database;
using Phoenix.Firebase.Auth;
using Phoenix.Firebase.RT;
using UnityEngine;

namespace Phoenix.Network
{
    public class NetworkManager : MonoBehaviour
    {
        private void Start()
        {
            RTManager.RTInstance.OnGameStart += OnGameStartEvent;
        }
        
        #region Match Making
        public void HostGame() => 
            RTManager.RTInstance.HostGame(AuthManager.AuthInstance.GetCurrentUser().UserId);
        public void JoinGame() => 
            RTManager.RTInstance.JoinGame(AuthManager.AuthInstance.GetCurrentUser().UserId);
        #endregion

        #region Events

        private void OnGameStartEvent()
        {
            Debug.Log("Game Started");
        }

        #endregion

    }
}