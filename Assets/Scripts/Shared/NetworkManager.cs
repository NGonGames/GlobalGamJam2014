using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	private const string typeName = "Game Designer -";
	private const string gameName = "RoomRoom";
	private float lastHostUpdate = 99999;
	private float lastNetworkLoadUpdate = 99999;
	public enum NetworkState:int {
		idle,
		matching,
		connecting,
		loading0,
		loading1,
		loading2,
		loading3,
		prepare0,
		prepare1,
		prepare2,
		ready0,
		ready1,
		playing
	};
	private MainMenu readyGUI = null;
	private MainMenu menuGUI = null;
	private MainMenu hudGUI = null;
	private MenuCamera gui = null;

	private SplineAnimator playerSpline = null;

	public int currentState;
	
	public bool localLoaded=false;
	public GameObject player;
	public GameObject spline;
	public float pollRate = 1;
	public SoundManager soundManager;

//GameObject Stuff
	void Update()
	{
		if(gui == null)  {
			gui = GameObject.Find("GUI").GetComponent<MenuCamera>();
		}
		switch(currentState) {
			case (int)NetworkState.idle:
				if(menuGUI == null){
					menuGUI = gui.AddMovie("MainMenu.swf");				
				}
				break;
			case (int)NetworkState.matching:
				MatchMake();
				break;
			case (int)NetworkState.loading0:
				LoadNetworkObjects();
				break;
			case (int)NetworkState.loading1:
			case (int)NetworkState.loading2:
				if(!localLoaded) {
					PairNetworkObjects();
				}
				break;
			case (int)NetworkState.loading3:
				//Delete Stuff From GameStart.
				GameObject.Destroy(GameObject.Find("MainCamera"));
				menuGUI.Destroy();
				currentState++;
				break;
			case (int)NetworkState.prepare0:
				if(Network.isServer) {
					readyGUI = gui.AddMovie("InstructionsGameDesigner.swf");
					hudGUI = gui.AddMovie("In_GameHUD.swf");
				} else {
					readyGUI = gui.AddMovie("InstructionsRobot.swf");
				}
				currentState++;
				break;
			case (int)NetworkState.ready0:
				if(GameObject.Find(player.name+"(Clone)").GetComponent<MenuCamera>() != null) {
					readyGUI.ReadyChecked();
				}
				currentState++;
				break;
			case (int)NetworkState.ready1:
				readyGUI.Destroy();
				currentState++;
				break;
			case (int)NetworkState.playing:
				if(playerSpline == null) {
					playerSpline = GameObject.Find(player.name+"(Clone)").GetComponent<SplineAnimator>();
				}
				if(Network.isClient) {
					playerSpline.paused = false;
				}
				if(playerSpline.passedTime>=1) {
					hudGUI.Destroy();
					gui.AddMovie("Robot_WinAndLooseScreens.swf");
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
					GameObject camGam = GameObject.Find("PlayerCamera");
					designerRef.cam = camGam.GetComponent<Camera>();
				}

				SplineAnimator tmpSpline = playerRef.GetComponent<SplineAnimator>();
				tmpSpline.spline = splineRef.GetComponent<Spline>();
				networkView.RPC("LoadingHandshake",RPCMode.All);
				localLoaded = true;
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
