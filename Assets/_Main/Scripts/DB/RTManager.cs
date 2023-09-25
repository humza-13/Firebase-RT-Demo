using Firebase;
using Firebase.Database;
using Phoenix.Firebase.Managers;
using UnityEngine;

namespace Phoenix.Firebase.RT
{
    public class RTManager : FirebaseManager
    {
        public static RTManager _rtInstance;

        protected override void Start()
        {
            base.Start();
            if (dependencyStatus == DependencyStatus.Available) Initialize();
            _rtInstance = this;
        }
        protected override void Initialize()
        {
            base.Initialize();
            
            if (dependencyStatus == DependencyStatus.Available)
            {
                AppOptions options = new AppOptions
                {
                    DatabaseUrl = new System.Uri("https://rt-demo-1d50e-default-rtdb.firebaseio.com/"),
                };
                app = FirebaseApp.Create(options);
            }

            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }
        
        public void HostGame(string playerUid)
        {
            string sessionId = GenerateUniqueSessionId();
            
            var sessionData = new GameSession()
            {
                player1_uid = playerUid,
                player2_uid = "",
                game_started = false
            };
            Debug.Log("Setting User Data");
            databaseReference.Child(sessionId).SetRawJsonValueAsync(JsonUtility.ToJson(sessionData));
            Debug.Log("Sent User Data");

        }
        private string GenerateUniqueSessionId()
        {
            return $"{System.DateTime.Now.Ticks}-{Random.Range(1000, 9999)}";
        }
        
    }
}