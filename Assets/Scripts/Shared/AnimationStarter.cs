using UnityEngine;
using System.Collections;

public class AnimationStarter : MonoBehaviour {

	public string animName;

	// Use this for initialization
	void Start () {
		animation.Play(animName);
	}
}
