public class RoomControllerOffline : RoomController
{
    public IPlayer AiPlayer     { get; private set; }

    protected override void OnRoomEntered()
    {
        base.OnRoomEntered();

        CreatePlayer();
        CreateAiPlayer(2);

        SetRoomStatus();
    }


    private void CreatePlayer()
    {
        NetworkPlayer netPlayer = new NetworkPlayer(1, PlayerDataController.Username);
        players.Add(netPlayer.GetId().ToString(), netPlayer);
        netPlayer.Init();

        LocalPlayer = netPlayer;
        
        PlayerEnteredRoom.Invoke(netPlayer);
    }

    private void CreateAiPlayer(int id)
    {
        AiPlayer = new AiPlayer(id);
        players.Add(AiPlayer.GetId().ToString(), AiPlayer);
        PlayerEnteredRoom.Invoke(AiPlayer);
        SetRoomStatus();
    }
    
    
}
