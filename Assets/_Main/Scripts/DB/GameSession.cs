using System;
using System.Collections.Generic;

namespace Phoenix.Firebase.RT
{
    [Serializable]
    public struct GameSession
    {
        public string player1_uid;
        public string player2_uid;
        public bool game_started;
    }

    [Serializable]
    public struct GameSessions
    {
        public Dictionary<string, GameSession> games;
    }

    }
