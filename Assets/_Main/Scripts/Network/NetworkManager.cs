using System;
using Firebase.Database;
using Phoenix.Firebase.Auth;
using Phoenix.Firebase.RT;
using UnityEngine;

namespace Phoenix.Network
{
    public class NetworkManager : MonoBehaviour
    {
        #region Match Making
        public void HostGame() => 
            RTManager._rtInstance.HostGame(AuthManager._authInstance.GetCurrentUser().UserId);
        public void JoinGame() => 
            RTManager._rtInstance.JoinGame(AuthManager._authInstance.GetCurrentUser().UserId);
        #endregion
        

    }
}