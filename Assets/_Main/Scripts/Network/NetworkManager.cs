using Phoenix.Firebase.Auth;
using Phoenix.Firebase.RT;
using UnityEngine;

namespace Phoenix.Network
{
    public class NetworkManager : MonoBehaviour
    {
        public void HostGame() => 
            RTManager._rtInstance.HostGame(AuthManager._authInstance.GetCurrentUser().UserId);
        
    }
}