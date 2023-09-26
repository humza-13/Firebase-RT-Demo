using System;
using Firebase;
using Firebase.Database;
using Phoenix.Firebase.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Phoenix.Firebase.RT
{
    public class RTManager : FirebaseManager
    {
        public static RTManager _rtInstance;
        public string CUURENT_SESSION;
        
        #region Overrides
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
                    DatabaseUrl = new System.Uri("https://rt-demo-2af6a-default-rtdb.asia-southeast1.firebasedatabase.app/"),
                };
                app = FirebaseApp.Create(options);
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            }

        }
        #endregion

        #region Hosting
        public void HostGame(string playerUid)
        {
            CUURENT_SESSION = GenerateUniqueSessionId();
            var sessionData = new GameSession()
            {
                player1_uid = playerUid,
                player2_uid = "",
                game_started = false
            };
            databaseReference.Child(CUURENT_SESSION).SetRawJsonValueAsync(JsonUtility.ToJson(sessionData));
            SetGameStartEvent();
        }
        private string GenerateUniqueSessionId()
        {
            return $"{System.DateTime.Now.Ticks}-{Random.Range(1000, 9999)}";
        }
        #endregion

        #region Joining
        public async void JoinGame(string currentPlayerUid)
        {
            DataSnapshot snapshot = await databaseReference.GetValueAsync();

            if (snapshot.Exists)
            {
                foreach (var session in snapshot.Children)
                {
                    CUURENT_SESSION = session.Key;
                    var sessionData = session.GetRawJsonValue();
                    GameSession gameSession = JsonUtility.FromJson<GameSession>(sessionData);
                    
                    if (!gameSession.game_started && string.IsNullOrEmpty(gameSession.player2_uid))
                    {
                        gameSession.player2_uid = currentPlayerUid;
                        gameSession.game_started = true;
                        
                        await databaseReference.Child(CUURENT_SESSION).SetRawJsonValueAsync(JsonUtility.ToJson(gameSession));
                        SetGameStartEvent();
                        return;
                    }
                }
            }
            HostGame(currentPlayerUid);
        }

        #endregion
        
        #region Events
        private void SetGameStartEvent()
        {
            DatabaseReference db = databaseReference
                .Child(CUURENT_SESSION).Child("game_started");
            
            db.ValueChanged += (sender, args) =>
            {
                if (args.DatabaseError != null)
                {
                    Debug.LogError("Error reading game_started: " + args.DatabaseError.Message);
                    return;
                }

                bool gameStarted = Convert.ToBoolean(args.Snapshot.Value);
    
                if (gameStarted)
                    Debug.Log("Game has started!");
            };
        }

        #endregion

        #region Helper

        public DatabaseReference GetDBReference()
        {
            return databaseReference;
        }
        
        #endregion
        
    }
}