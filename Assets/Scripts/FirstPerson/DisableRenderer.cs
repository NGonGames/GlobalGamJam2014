using UnityEngine;
using System.Collections;

public class DisableRenderer : MonoBehaviour {

	void Start () {
		if (Network.isClient)
			foreach (Renderer r in GetComponentsInChildren<Renderer>())
				r.enabled = false;
	}
}
