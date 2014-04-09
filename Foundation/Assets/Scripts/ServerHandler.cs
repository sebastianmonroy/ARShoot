using UnityEngine;
using System.Collections;

public class ServerHandler : MonoBehaviour {
	public int MAX_PLAYERS = 2;
	private const string typeName = "AR Network Sample";
    private const string gameName = "Test Game";
	private int connectPort = 25001;
	private int NUM_PLAYERS = 0;
	private string txtfield = "1";
	private bool button;

	void OnGUI() {
		txtfield = GUI.TextField(new Rect(10, 50, 100, 20), txtfield, 1);
		button = GUI.Button(new Rect(120, 50, 100, 20), "Start Server");

		if (button) {
			int.TryParse(txtfield, out NUM_PLAYERS);
			if (NUM_PLAYERS >= 1 && NUM_PLAYERS <= MAX_PLAYERS) {
				StartServer();
			}
		}
	}


	void Start() {
		//StartServer();
	}

	private void StartServer()
    {
    	MasterServer.ipAddress = "127.0.0.1";
    	MasterServer.port = 23466;
    	Network.natFacilitatorIP = "127.0.0.1";
    	Network.natFacilitatorPort = connectPort;

        Network.InitializeServer(10, connectPort, !Network.HavePublicAddress());
		MasterServer.dedicatedServer = true;
        MasterServer.RegisterHost(typeName, gameName);
    }

    void OnServerInitialized() 
	{
		Debug.Log("Server " + MasterServer.ipAddress + ":" + MasterServer.port + " initialized and ready");
	}

	void OnPlayerConnected(NetworkPlayer player) 
	{
		Debug.Log("Player connected from: " + player.ipAddress + ":" + player.port);
		GameHandler.Instance.AddPlayerToServer(player);
		if (GameHandler.Instance.NUM_PLAYERS == this.NUM_PLAYERS) {
			Debug.Log("All players connected to server");
			GameHandler.Instance.AddPlayersToClients();
		}
	}

	void OnPlayerDisconnected(NetworkPlayer player) 
	{
		Debug.Log("Player disconnected from: " + player.ipAddress + ":" + player.port);
	}
}
