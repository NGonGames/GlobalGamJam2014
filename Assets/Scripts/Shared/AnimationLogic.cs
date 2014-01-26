using UnityEngine;
using System.Collections;

public class AnimationLogic : MonoBehaviour {

	public Animator anim;
	public SplineCharacterController character;
	
	// Update is called once per frame
	void Update () {
		anim.SetFloat("height", character.transform.localPosition.y);
	}
}
