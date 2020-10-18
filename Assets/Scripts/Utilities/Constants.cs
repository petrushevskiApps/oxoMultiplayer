namespace com.petrushevskiapps.Oxo.Utilities
{
    public static class Constants
    {
        public const string PLAYER_READY_KEY = "playerReady";

        public const string ROOM_EXIST_TITLE_TXT = "ROOM EXISTS";
        public const string ROOM_EXIST_MESSAGE_TXT = "Room with name: <b>{0}</b> already exists!\n Please enter another room name and try again.";
        public const string ROOM_NOT_EXIST_TITLE_TXT = "WRONG ROOM NAME";
        public const string ROOM_NOT_EXIST_MESSAGE_TXT = "Room with name: <b>{0}</b> does not exist!\n Please enter another room name and try again.";
        
        public const string WAITING_PLAYERS_MESSAGE = "Waiting for players to join your game...";
        public const string PLAYERS_NOT_READY_MESSAGE = "Waiting for players to be ready for game...";
        public const string PLAYERS_READY_MESSAGE = "All players ready. You can start the game now.";
        
    }

    public static class Keys
    {
        public const string KEY_USERNAME = "username";
        public const string KEY_PLAYED_GAMES = "playedGames";
        public const string KEY_WON_GAMES = "gamesWon";
        public const string KEY_LOST_GAMES = "gamesLost";
        public const string KEY_BG_MUSIC_STATUS = "backgroundMusic";
        public const string KEY_SFX_STATUS = "sfxStatus";
        public const string KEY_VIBRATION_STATUS = "vibrationStatus";
    }
}