using UnityEngine;
using System.Collections;

public class TopDownCamera : MonoBehaviour {
	
	public Transform player;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// follow the other player.
		if(player != null) {			
			Camera.main.transform.position = new Vector3(player.position.x, 10, player.position.z);
		}
	}
}
