using UnityEngine;
using System.Collections;

public class Designer : MonoBehaviour {

	public Abilities currentAbility;

	public GameObject spike;
	public GameObject mine;
	public GameObject lazor;

	public enum Abilities:int {
		Spike,
		Mine,
		Lazor
	}

	[HideInInspector]
	public Spline spline;
	[HideInInspector]
	public Camera cam;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("SpikeAbility"))
			currentAbility = Abilities.Spike;
		if (Input.GetButtonDown("MineAbility"))
			currentAbility = Abilities.Mine;
		if (Input.GetButtonDown("LazorAbility"))
			currentAbility = Abilities.Lazor;


		if (Input.GetMouseButtonDown(0) && Network.isServer && spline != null)
		{
			Ray rayHitTest = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			//Debug.DrawRay(rayHitTest);
			if(Physics.Raycast(rayHitTest, out hit, 1000, 1 << LayerMask.NameToLayer("Spline")))
			{
				float param = spline.GetClosestPointParamToRay(rayHitTest,10);
				switch (currentAbility) {
					case Abilities.Spike:
						placeSpike(param);
						break;
					case Abilities.Mine:
						placeMine(param);
						break;
					case Abilities.Lazor:
						placeLazor(param);
						break;
				}
			}
		}
	}

	private void placeSpike(float param) {
		Vector3 pos = spline.GetPositionOnSpline(param);
		pos.Set(pos.x, pos.y + .1f, pos.z);
		Quaternion placeRotation = spline.GetOrientationOnSpline(param);
		Quaternion adjust = Quaternion.Euler(270, 0, 0);
		Network.Instantiate(spike, pos, placeRotation * adjust, 0);
	}

	private void placeMine(float param) {
		Vector3 pos = spline.GetPositionOnSpline(param);
		pos.Set (pos.x, pos.y + -.27f, pos.z);
		Quaternion rot = spline.GetOrientationOnSpline(param);
		Quaternion adjust = Quaternion.Euler(0, 0, 0);
		Network.Instantiate(mine, pos, rot * adjust, 0);
	}

	private void placeLazor(float param) {
		Vector3 pos = spline.GetPositionOnSpline(param);
		pos.Set (pos.x, pos.y + 0f, pos.z);
		Quaternion rot = spline.GetOrientationOnSpline(param);
		Quaternion adjust = Quaternion.Euler(0, 0, 0);
		Network.Instantiate(lazor, pos, rot * adjust, 0);
	}
}
