using UnityEngine;
using System.Collections;

public class ServerHandler : MonoBehaviour {
	public int MAX_PLAYERS = 2;
	private const string typeName = "AR Network Sample";
    private const string gameName = "Test Game";
    private string connectIP = "127.0.0.1";
	private int connectPort = 25001;
	private int NUM_PLAYERS = 0;
	private string playersField = "1";
	private bool button;

	void OnGUI() {
		connectIP = GUILayout.TextField(connectIP);
		button = GUILayout.Button("Start Server");
		playersField = GUILayout.TextField(playersField);
		if (button) {
			int.TryParse(playersField, out NUM_PLAYERS);
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
    	MasterServer.ipAddress = connectIP;
    	MasterServer.port = 23466;
    	Network.natFacilitatorIP = connectIP;
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
