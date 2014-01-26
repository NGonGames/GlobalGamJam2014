using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform client;
    public Transform server;
	public float height = 5f;

	void LateUpdate () {
		if (Network.isServer) {
			transform.position = new Vector3(server.position.x, server.position.y + height, server.position.z);
		} else {
			transform.position = client.position;
			transform.rotation = client.rotation;
		}
	}
}
