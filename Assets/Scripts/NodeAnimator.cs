using UnityEngine;
using System.Collections;

public class NodeAnimator : MonoBehaviour {

	[Range(0, 1)]
	public float speed = 1f;
	public float height = 1f;
	public float width = 1f;
	public bool animateX = true;
	public bool animateY = false;

	private float counter = 0f;
	private Vector2 offset;
	private Vector3 start;
	
	void Start () {
		offset = new Vector2();
		start = transform.position;
	}

	void Update () {

		counter += Time.deltaTime;

		if (animateY)
			offset.y = Mathf.Sin(counter) * height;
		if (animateX)
			offset.x = Mathf.Cos(counter) * width;

		transform.position = new Vector3(start.x + offset.x, start.y + offset.y, transform.position.z);

	}
}
