using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour {

	private bool fadingOut = false;
	private bool fadingIn = false;
	private float start;
	public float duration = 5f;
	private float alpha = 0;
	private int levelToLoad = 0;

	// Use this for initialization
	void Start () {
		StartFadeIn ();
	}

	public void StartFadeOut(int level)
	{
		if (!fadingIn && !fadingOut) {
			if (!fadingOut)
				start = Time.time;
			fadingOut = true;
			levelToLoad = level;
		}
	}

	public void StartFadeIn()
	{
		if (!fadingIn && !fadingOut) {
			if (!fadingIn)
				start = Time.time;
			fadingIn = true;
		}
	}

	// Update is called once per frame
	void Update () {

		if (fadingOut) {
			alpha = (Time.time - start) / duration;
			Debug.Log(alpha);
			gameObject.GetComponent<Image>().color = new Color(1,1,1,alpha);
			GameObject.Find("Main Camera").gameObject.GetComponent<AudioSource>().volume = 1 - alpha;
			if(alpha > 1)
			{
				Application.LoadLevel(levelToLoad);
			}
		}

		if (fadingIn) {
			alpha = (Time.time - start) / duration;
			Debug.Log(alpha);
			gameObject.GetComponent<Image>().color = new Color(1,1,1, 1 - alpha);
			GameObject.Find("Main Camera").gameObject.GetComponent<AudioSource>().volume = alpha;
			if(alpha >= 1)
			{
				gameObject.GetComponent<Image>().color = new Color(1,1,1, 0);
				GameObject.Find("Main Camera").gameObject.GetComponent<AudioSource>().volume = 1;
				fadingIn = false;
			}
		}

	}
}
