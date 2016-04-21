using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class LevelChange : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler  {

	public bool isSelected = false;
	public int levelToLoad = 0;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () {
		if (isSelected) {
			if (Input.GetAxis ("RightTrigger") > 0) 
			{
				GameObject.Find("BlackPanel").GetComponent<Fade>().StartFadeOut(levelToLoad);
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventdata)
	{
	}
	public void OnSelect(BaseEventData eventdata)
	{
		isSelected = true;

	}

	public void OnDeselect(BaseEventData eventdata)
	{
		isSelected = false;
	}
}
