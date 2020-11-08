namespace com.petrushevskiapps.Oxo.Utilities
{
    public static class Constants
    {
        public const string LEAVE_MATCH_TITLE = "Leave Match";
        public const string LEAVE_MATCH_MESSAGE = "Are you sure?\nLeaving the match will result with your opponent winning!";
        
        public const string LEAVE_ROOM_TITLE = "Leave Room";
        public const string LEAVE_ROOM_MESSAGE = "Are you sure you want to leave this room?";


        public const string ROOM_EXIST_TITLE_TXT = "ROOM EXISTS";
        public const string ROOM_EXIST_MESSAGE_TXT = "Room with name: <b>{0}</b> already exists!\n Please enter another room name and try again.";
        public const string ROOM_NOT_EXIST_TITLE_TXT = "WRONG ROOM NAME";
        public const string ROOM_NOT_EXIST_MESSAGE_TXT = "Room with name: <b>{0}</b> does not exist!\n Please enter another room name and try again.";
        
        public const string WAITING_PLAYERS_MESSAGE = "Waiting for players to join your game...";
        public const string PLAYERS_NOT_READY_MESSAGE = "Waiting for players to be ready for game...";
        public const string PLAYERS_READY_MESSAGE = "All players ready. You can start the game now.";

        public const string TIMER_POPUP_MESSAGE = "Waiting for {playerName} to rejoin the game.";

    }

    public static class Keys
    {
        public const string PLAYER_READY_KEY = "playerReady";
        public const string PLAYER_MATCH_ID = "playerMatchId";
        public const string PLAYER_MATCH_SCORE = "playerScore";
        
        public const string ROOM_STATUS = "roomStatus";
        public const string ROOM_STATE  = "roomState";
        
        public const string USERNAME = "username";
        public const string PLAYED_GAMES = "playedGames";
        public const string WON_GAMES = "gamesWon";
        public const string LOST_GAMES = "gamesLost";
        public const string BG_MUSIC_STATUS = "backgroundMusic";
        public const string SFX_STATUS = "sfxStatus";
        public const string VIBRATION_STATUS = "vibrationStatus";
    }

    public static class RPCs
    {
        public const string RPC_START_MATCH = "StartMatchSynced";
    }
}