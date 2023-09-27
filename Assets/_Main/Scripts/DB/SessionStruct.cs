using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Phoenix.Firebase.RT
{
    [Serializable]
    public struct SessionStruct
    {
        public string player1_uid;
        public string player2_uid;
        public bool game_started;
    }

    [Serializable]
    public struct GameSessions
    {
        public Dictionary<string, SessionStruct> sessions;
    }

    }
