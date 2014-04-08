using UnityEngine;
using System.Collections;

public class NetworkHandler : MonoBehaviour {
	public bool isServer;
	private const string typeName = "AR Network Sample";
    private const string gameName = "Test Game";
	private int connectPort = 25001;

	void Start() {
		if (this.isServer) {
			StartServer();
		} else {
			//JoinServer();
			this.gameObject.GetComponent<ConnectGUI>.active = true;
		}
	}

	private void StartServer()
    {
        Network.InitializeServer(10, connectPort, !Network.HavePublicAddress());
		MasterServer.dedicatedServer = true;
        MasterServer.RegisterHost(typeName, gameName);
    }

    private void JoinServer()
    {
        Network.Connect("0.0.0.0", connectPort);
    }

    void OnServerInitialized() 
	{
		Debug.Log("Server " + MasterServer.ipAddress + ":" + MasterServer.port + " initialized and ready");
	}

	void OnConnectedToServer() 
	{
		Debug.Log ("This CLIENT has connected to a server");
	}
}
