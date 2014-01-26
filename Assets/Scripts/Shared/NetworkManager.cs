using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	private const string typeName = "Game Designer";
	private const string gameName = "RoomRoom";
	private float lastHostUpdate = 99999;
	private float lastNetworkLoadUpdate = 99999;
	private enum NetworkState:int {
		idle,
		matching,
		connecting,
		loading0,
		loading1,
		prepare0,
		prepare1,
		ready,
		playing
	};
	private int currentState;

	public GameObject player;
	public GameObject spline;
	public float pollRate = 1;
	
	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Game Designer"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Player"))
				currentState = (int)NetworkState.matching;
				//TODO Show matchmaking screen.
			

		}
	}

//GameObject Stuff
	void Start()
	{
		DontDestroyOnLoad(transform.gameObject);
	}
	void Update()
	{
		switch(currentState) {
			case (int)NetworkState.matching:
				MatchMake();
				break;
			case 4:
				PairNetworkObjects();
				break;
		}
	}

//Matchmaking Stuff
	private void MatchMake()
	{
			if (MasterServer.PollHostList().Length > 0)
			{
				currentState = (int)NetworkState.connecting;
				Network.Connect(MasterServer.PollHostList()[0]);
			}
			if (lastHostUpdate>pollRate) {
				MasterServer.RequestHostList(typeName);
				lastHostUpdate = 0;
			}
			lastHostUpdate += Time.deltaTime;
	}
	private void StartServer()
	{
		Network.maxConnections = 2;
		Network.InitializeServer(5, 1337, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}
	private void PairNetworkObjects(){
		
		if (lastNetworkLoadUpdate>0.1) {
			GameObject playerRef = GameObject.Find(player.name+"(Clone)");
			GameObject splineRef = GameObject.Find(spline.name+"(Clone)");
			if(playerRef != null && splineRef != null)
			{
				SplineAnimator tmpSpline = playerRef.GetComponent<SplineAnimator>();
				tmpSpline.spline = splineRef.GetComponent<Spline>();
				networkView.RPC("LoadingHandshake",RPCMode.All);
			}
			lastNetworkLoadUpdate = 0;
		}
		lastNetworkLoadUpdate += Time.deltaTime;
	}
	[RPC]
	private void LoadingHandshake() {
		//the other person has loaded the level successfully.
		currentState++;
	}
	[RPC]
	private void ReadyHandshake() {
		//the other person is ready start the game
		currentState++;
	}
	void OnLevelWasLoaded () {
		if(Network.isServer){			
			Network.Instantiate(spline,Vector3.one,Quaternion.identity,0);
		} else {			
			Network.Instantiate(player,Vector3.one,Quaternion.identity,0);
		}
	}
//Server Stuff
	void OnPlayerConnected()
	{
		//Create the spline.
		//Set the Player's Spline
		Application.LoadLevel("Game-Designer");
		currentState = (int)NetworkState.loading0;
		//TODO finish the loading screen.
		//TODO show the ready screen.
	}
	void OnServerInitialized()
	{
		//TODO show the loading screen.
	}

//Client Stuff
	void OnConnectedToServer()
	{
		Application.LoadLevel("Game-Player");
		//TODO Create Player
		currentState = (int)NetworkState.loading0;
		//TODO Show the Ready screen.
	}
}
