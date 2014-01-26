using UnityEngine;
using System.Collections;

public class KillPlayer : MonoBehaviour {

	private GameObject player;

	void Start() {
		player = GameObject.Find("PlayerRail(Clone)");
	}

	void OnTriggerEnter(Collider col) {
		SplineAnimator spAnim = player.GetComponent<SplineAnimator>();
		spAnim.passedTime = 0;
	}
}
