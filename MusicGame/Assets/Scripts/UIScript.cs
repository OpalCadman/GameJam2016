using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//function that will set up UI text on screen with the given text, position and canvas
	public GameObject setupUIText(string name, string text, Vector2 anch, Vector3 anchPos, bool scale)
	{
		//setting up the UI object
		GameObject uiObject = Instantiate(Resources.Load("UIText"), 
		                                  this.GetComponent<RectTransform>().position, 
		                                  this.GetComponent<RectTransform>().rotation) as GameObject;
		//setting the values to those that are given 
		//(objects name, parent, anchor, anchor position and text)
		uiObject.name = name;
		uiObject.GetComponent<RectTransform>().SetParent(this.GetComponent<RectTransform>(), !scale);
		uiObject.GetComponent<RectTransform> ().anchorMin = anch;
		uiObject.GetComponent<RectTransform> ().anchorMax = anch;
		uiObject.GetComponent<RectTransform> ().pivot = anch;
		uiObject.GetComponent<RectTransform>().anchoredPosition = anchPos;
		uiObject.GetComponent<Text>().text = text;
		return uiObject;
	}
	
	//function that will set up UI image on screen with the given source, position and canvas
	public GameObject setupUIImage(string name, string src, Vector2 anch, Vector3 anchPos, bool scale)
	{
		//setting up the UI object
		GameObject uiObject = Instantiate(Resources.Load("UIImage"), 
		                                  this.GetComponent<RectTransform>().position, 
		                                  this.GetComponent<RectTransform>().rotation) as GameObject;
		//setting the values to those that are given 
		//(objects name, parent, anchor, anchor position and source)
		uiObject.name = name;
		uiObject.GetComponent<RectTransform>().SetParent(this.GetComponent<RectTransform>(), !scale);
		uiObject.GetComponent<RectTransform> ().anchorMin = anch;
		uiObject.GetComponent<RectTransform> ().anchorMax = anch;
		uiObject.GetComponent<RectTransform> ().pivot = anch;
		uiObject.GetComponent<RectTransform>().anchoredPosition = anchPos;
		uiObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(src);
		return uiObject;
	}
}