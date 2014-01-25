using UnityEngine;
using System.Collections;

public class Designer : MonoBehaviour {

	public GameObject ability1;

	public GameObject map;
	public Spline spline;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && Network.isServer)
		{
			Ray rayHitTest = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(rayHitTest,out hit, 1000))
			{
				if (hit.transform.gameObject.Equals(spline.gameObject))
				{
					float param = spline.GetClosestPointParamToRay(rayHitTest,10);
					Vector3 placePosition = transform.position = spline.GetPositionOnSpline( param );
					Quaternion placeRotation = transform.rotation = spline.GetOrientationOnSpline( param );
					Network.Instantiate(ability1, placePosition, placeRotation, 0);
				}
			}
		}
	}
}
