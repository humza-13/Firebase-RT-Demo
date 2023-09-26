using Phoenix.Firebase.Auth;
using Phoenix.Firebase.RT;
using UnityEngine;

namespace Phoenix.Network
{
    [RequireComponent(typeof(AuthManager))]
    [RequireComponent(typeof(RTManager))]
    public class NetworkManager : MonoBehaviour
    {
        public void HostGame() => 
            RTManager._rtInstance.HostGame(AuthManager._authInstance.GetCurrentUser().UserId);
        public void JoinGame() => 
            RTManager._rtInstance.JoinGame(AuthManager._authInstance.GetCurrentUser().UserId);
        
    }
}