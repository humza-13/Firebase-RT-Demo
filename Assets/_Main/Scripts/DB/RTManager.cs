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
        #region Attributes
        public static RTManager RTInstance;
        public string currentSession;
        public event Action OnGameStart;
        #endregion
        
        #region Overrides
        protected override void Start()
        {
            base.Start();
            if (DependencyStatus == DependencyStatus.Available) Initialize();
            RTInstance = this;
        }
        protected override void Initialize()
        {
            base.Initialize();
            
            if (DependencyStatus == DependencyStatus.Available)
            {
                AppOptions options = new AppOptions
                {
                    DatabaseUrl = new System.Uri("https://rt-demo-2af6a-default-rtdb.asia-southeast1.firebasedatabase.app/"),
                };
                App = FirebaseApp.Create(options);
                DatabaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            }

        }
        #endregion

        #region Hosting
        public void HostGame(string playerUid)
        {
            currentSession = GenerateUniqueSessionId();
            var sessionData = new GameSession()
            {
                player1_uid = playerUid,
                player2_uid = "",
                game_started = false
            };
            DatabaseReference.Child(currentSession).SetRawJsonValueAsync(JsonUtility.ToJson(sessionData));
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
            DataSnapshot snapshot = await DatabaseReference.GetValueAsync();

            if (snapshot.Exists)
            {
                foreach (var session in snapshot.Children)
                {
                    currentSession = session.Key;
                    var sessionData = session.GetRawJsonValue();
                    GameSession gameSession = JsonUtility.FromJson<GameSession>(sessionData);
                    
                    if (!gameSession.game_started && string.IsNullOrEmpty(gameSession.player2_uid))
                    {
                        gameSession.player2_uid = currentPlayerUid;
                        gameSession.game_started = true;
                        
                        await DatabaseReference.Child(currentSession).SetRawJsonValueAsync(JsonUtility.ToJson(gameSession));
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
            DatabaseReference db = DatabaseReference
                .Child(currentSession).Child("game_started");
            
            db.ValueChanged += (sender, args) =>
            {
                if (args.DatabaseError != null)
                {
                    Debug.LogError("Error reading game_started: " + args.DatabaseError.Message);
                    return;
                }

                bool gameStarted = Convert.ToBoolean(args.Snapshot.Value);
                if (gameStarted)
                    OnGameStart?.Invoke();
            };
        }

        #endregion

        #region Helper

        public DatabaseReference GetDBReference()
        {
            return DatabaseReference;
        }
        
        #endregion
        
    }
}