using UnityEngine;
using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	private const string typeName = "Game Designer";
	private const string gameName = "RoomRoom";
	private float lastUpdate = 0;
	private bool matchMaking = false;

	public GameObject playerPrefab;
	public float pollRate = 1;
	
	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Game Designer"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Player"))
				matchMaking = true;
				//Show matchmaking screen.
			

		}
	}

//GameObject Stuff
	void Start()
	{
		DontDestroyOnLoad(transform.gameObject);
	}
	void Update()
	{
		MatchMake();
	}

//Matchmaking Stuff
	private void MatchMake()
	{
		if (matchMaking)
		{			
			if (MasterServer.PollHostList().Length > 0)
			{
				Network.Connect(MasterServer.PollHostList()[0]);
			}
			if (lastUpdate>pollRate) {
				MasterServer.RequestHostList(typeName);
				lastUpdate = 0;
			}
			lastUpdate += Time.deltaTime;
		}
	}
	private void StartServer()
	{
		Network.maxConnections = 2;
		Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

//Server Stuff
	void OnPlayerConnected()
	{
		//TODO finish the loading screen.
		//TODO Load network objects.
		//Network.Instantiate(playerPrefab);
	}
	void OnServerInitialized()
	{
		//TODO show the loading screen.
		//Load Level except network objects.
		Application.LoadLevel("Game-Designer");
	}

//Client Stuff
	void OnConnectedToServer()
	{
		matchMaking = false;
		Application.LoadLevel("Game-Player");
	}
}
