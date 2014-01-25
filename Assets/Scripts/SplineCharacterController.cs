using UnityEngine;
using System.Collections;

public class SplineCharacterController : MonoBehaviour {

    [Range(0, 1)]
    public float horizontalSpeed = 0.2f;
    public float offsetThreshold = 0.5f;
    public float jumpHeight = 1f;
    public float gravity = 0.1f;
	public float ystart = 0.2f;
	public AudioSource audioSource;

    private float offset = 0f;
    private float height = 0f;
    private float yspeed = 0f;
    
	// Update is called once per frame
	void Update () {

        float h = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump")) {
            yspeed = jumpHeight;
        }

        yspeed -= gravity;
        height += yspeed;

        if (height < 0f) {
            height = 0f;
            yspeed = 0f;
        }

		if (height != 0) {
			audioSource.volume = 0;
		} else {
			audioSource.volume = 1;
		}

        offset += h * horizontalSpeed;
        if (Mathf.Abs(offset) > offsetThreshold) {
            offset = offsetThreshold * Mathf.Sign(offset);
        }
        transform.localPosition = new Vector3(offset, ystart + height, 0f);

	}
}
