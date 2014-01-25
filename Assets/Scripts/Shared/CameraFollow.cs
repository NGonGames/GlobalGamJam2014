using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;

	void LateUpdate () {
		if (Network.isServer) {
			transform.position = new Vector3(target.position.x, target.position.y + 5, target.position.z);
		} else {
			transform.position = target.position;
			transform.rotation = target.rotation;
		}
	}
}
