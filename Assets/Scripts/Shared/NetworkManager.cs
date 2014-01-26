using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	private const string typeName = "Game Designer - Ben";
	private const string gameName = "RoomRoom";
	private float lastHostUpdate = 99999;
	private float lastNetworkLoadUpdate = 99999;
	private enum NetworkState:int {
		idle,
		matching,
		connecting,
		loading0,
		loading1,
		loading2,
		loading3,
		prepare0,
		prepare1,
		ready,
		playing
	};
	private int currentState;

	public GameObject player;
	public GameObject spline;
	public float pollRate = 1;
	public SoundManager soundManager;

//GameObject Stuff
	void Update()
	{
		switch(currentState) {
			case (int)NetworkState.matching:
				MatchMake();
				break;
			case (int)NetworkState.loading0:
				LoadNetworkObjects();
				break;
			case (int)NetworkState.loading1:
			case (int)NetworkState.loading2:
				PairNetworkObjects();
				break;
			case (int)NetworkState.loading3:
				//Delete Stuff From GameStart.
				foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("Menu")) {
					Object.Destroy(gameObject);
				}
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

	private void PairNetworkObjects() {
		
		if (lastNetworkLoadUpdate>0.1) {
			GameObject playerRef = GameObject.Find(player.name+"(Clone)");
			GameObject splineRef = GameObject.Find(spline.name+"(Clone)");
			if(playerRef != null && splineRef != null)
			{
				if(Network.isServer) {
					Designer designerRef = GameObject.Find("Designer").GetComponent<Designer>();
					designerRef.spline = splineRef.GetComponent<Spline>();
					designerRef.cam = GameObject.Find("Camera").GetComponent<Camera>();
				}

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
	void LoadNetworkObjects () {
		if(Network.isServer){			
			Network.Instantiate(spline,Vector3.one,Quaternion.identity,0);
		} else {			
			Network.Instantiate(player,Vector3.one,Quaternion.identity,0);
		}
		currentState++;
	}
//Server Stuff
	
	public void StartServer()
	{
		Network.maxConnections = 2;
		Network.InitializeServer(5, 1337, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	void OnPlayerConnected()
	{
		//Create the spline.
		//Set the Player's Spline
		soundManager.DesignerTheme();
		Application.LoadLevelAdditiveAsync("Game-Designer");
		currentState = (int)NetworkState.loading0;
		//TODO finish the loading screen.
		//TODO show the ready screen.
	}
	void OnServerInitialized()
	{
		//TODO show the loading screen.
	}

//Client Stuff
	public void StartClient() {		
		currentState = (int)NetworkState.matching;
	}
	void OnConnectedToServer()
	{
		soundManager.RobotTheme();
		Application.LoadLevelAdditiveAsync("Game-Player");
		//TODO Create Player
		currentState = (int)NetworkState.loading0;
		//TODO Show the Ready screen.
	}
}
