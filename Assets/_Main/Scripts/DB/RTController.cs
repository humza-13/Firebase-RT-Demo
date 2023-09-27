using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Phoenix.Firebase.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Phoenix.Firebase.RT
{
    public class RTController : FirebaseController
    {
        public string currentSession;
        
        #region Actions
        public event Action OnGameStart;
        public event Action<string> OnPlayerJoin; 
        public event Action<string> OnPlayerLeft;
     
        #endregion

        #region Singleton
        private static RTController _rtInstance;
        public static RTController RTInstance
        {
            get { return _rtInstance; }
            private set { _rtInstance = value; }
        }
        private void Awake()
        {
            if (_rtInstance != null && _rtInstance != this)
            {
                Destroy(gameObject);
                return;
            }

            RTInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion
       
        #region Overrides
        protected override void Start()
        {
            base.Start();
            if (DependencyStatus == DependencyStatus.Available) Initialize();
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
            var sessionData = new SessionStruct()
            {
                player1_uid = "",
                player2_uid = "",
                game_started = false
            };
            DatabaseReference.Child(currentSession).SetRawJsonValueAsync(JsonUtility.ToJson(sessionData));
            PlayerListener();
            GameStartListener();
            var p1 = new SessionStruct()
            {
                player1_uid = playerUid,
                player2_uid = "",
                game_started = false
            };
            DatabaseReference.Child(currentSession).SetRawJsonValueAsync(JsonUtility.ToJson(p1));
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
                    SessionStruct sessionStruct = JsonUtility.FromJson<SessionStruct>(sessionData);
                    if (!sessionStruct.game_started && string.IsNullOrEmpty(sessionStruct.player2_uid))
                    {
                        PlayerListener();
                        GameStartListener();
                        sessionStruct.player2_uid = currentPlayerUid;
                        await DatabaseReference.Child(currentSession).SetRawJsonValueAsync(JsonUtility.ToJson(sessionStruct));
                        
                        sessionStruct.game_started = true;
                        await DatabaseReference.Child(currentSession).SetRawJsonValueAsync(JsonUtility.ToJson(sessionStruct));
                        
                        return;
                    }
                }
            }
            HostGame(currentPlayerUid);
        }

        #endregion
        
        #region Listeners
        private void GameStartListener()
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

        private void PlayerListener()
        {
            List<DatabaseReference> players = new List<DatabaseReference>();
            players.Add(DatabaseReference
                .Child(currentSession).Child("player1_uid"));
            players.Add(DatabaseReference
                .Child(currentSession).Child("player2_uid"));
            foreach (var player in players)
            {
                player.ValueChanged += (sender, args) =>
                {
                    if (args.DatabaseError != null)
                    {
                        Debug.LogError("Error reading game_started: " + args.DatabaseError.Message);
                        return;
                    }

                    string uid = Convert.ToString(args.Snapshot.Value);
                    if(!string.IsNullOrEmpty(uid))
                        OnPlayerJoin?.Invoke(uid);
                    
                };
            }

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