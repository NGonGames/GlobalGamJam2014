using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioClip menuTheme;
	public AudioClip designerTheme;
	public AudioClip robotTheme;

	public AudioSource source;

	// Use this for initialization
	void Start () {
		source.clip = menuTheme;
		source.Play();
	}

	public void MenuTheme() {
		source.clip = menuTheme;
		source.Play();
	}

	public void DesignerTheme() {
		source.clip = designerTheme;
		source.Play();
	}

	public void RobotTheme() {
		source.clip = robotTheme;
		source.Play();
	}

}
