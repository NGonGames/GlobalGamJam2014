using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    [Range(0, 1)]
    public float horizontalSpeed = 0.2f;
    public float offsetThreshold = 0.5f;
    public float jumpHeight = 1f;
    public float gravity = 0.1f;

    private float offset = 0f;
    private float height = 0f;
    private float yspeed = 0f;
    
	// Update is called once per frame
	void Update () {

        float h = Input.GetAxis("Horizontal");

        if (Input.GetButton("Jump")) {
            yspeed = jumpHeight;
        }

        yspeed -= gravity;
        height += yspeed;

        if (height < 0f) {
            height = 0f;
            yspeed = 0f;
        }

        offset += h * horizontalSpeed;
        if (Mathf.Abs(offset) > offsetThreshold) {
            offset = offsetThreshold * Mathf.Sign(offset);
        }
        transform.localPosition = new Vector3(offset, 0.2f + height, 0f);

	}
}
