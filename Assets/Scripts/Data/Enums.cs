﻿namespace Data
{
    public class EnumData
    {
        
    }
    
    public enum RoomStatus
    {
        Waiting,
        Full,
        Ready,
        MatchStarted,
    }

    public enum RoomState
    {
        InRoom,
        InGame
    }
    
    public enum TileType
    {
        Empty,
        Cross,
        Circle,
        Win,
        Lose,
    }
    
    public enum SceneTypes
    {
        Menu = 0,
        Game = 1,
    }
}