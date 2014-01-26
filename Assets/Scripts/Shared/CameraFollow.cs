using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float height = 5f;

	void LateUpdate () {
		if (Network.isServer) {
			transform.position = new Vector3(target.position.x, target.position.y + height, target.position.z);
		} else {
			transform.position = target.position;
			transform.rotation = target.rotation;
		}
	}
}
