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
            RTController.RTInstance.OnPlayerJoin += OnPlayerJoin;
            RTController.RTInstance.OnPlayerLeft += OnPlayerLeft;
            RTController.RTInstance.OnGameStart += OnGameStartEvent;
        }
        
        #region Match Making
        public void HostGame() => 
            RTController.RTInstance.HostGame(AuthController.AuthInstance.GetCurrentUser().UserId);
        public void JoinGame() => 
            RTController.RTInstance.JoinGame(AuthController.AuthInstance.GetCurrentUser().UserId);
        #endregion

        #region Events

        protected virtual void OnGameStartEvent()
        {
            Debug.Log("Game Started");
        }

        protected virtual void OnPlayerJoin(string uuid)
        {
            Debug.Log("Player: " + uuid + " joined");
        }
        protected virtual void OnPlayerLeft(string uuid)
        {
            Debug.Log("Player: " + uuid + " left");
        }
        
        #endregion

    }
}