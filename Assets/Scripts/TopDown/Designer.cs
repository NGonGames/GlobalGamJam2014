using UnityEngine;
using System.Collections;

public class Designer : MonoBehaviour {

	public GameObject ability1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && Network.isServer)
		{
			Vector3 placePosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.nearClipPlane));
			placePosition = new Vector3(Mathf.Round(placePosition.x), 1, Mathf.Round(placePosition.z));
			Network.Instantiate(ability1, placePosition, ability1.transform.rotation, 0);
		}
	}
}
