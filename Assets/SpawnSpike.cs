using UnityEngine;
using System.Collections;

public class SpawnSpike : MonoBehaviour {

	public Transform spikePrefab;

	// Use this for initialization
	void Start () {
		Object.Instantiate(spikePrefab, transform.position, transform.rotation);
	}

}
